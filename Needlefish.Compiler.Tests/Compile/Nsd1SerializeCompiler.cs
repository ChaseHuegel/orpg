using Needlefish.Compiler.Tests.Schema;
using System;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal class Nsd1SerializeCompiler : INsdTypeCompiler
{
    private const string SerializeTemplate = 
@"public byte[] Serialize()
{
    byte[] buffer = new byte[GetSize()];
    SerializeInto(buffer, 0);
    return buffer;
}";

    private const string SerializeIntoTemplate =
@"public unsafe void SerializeInto(byte[] buffer, int start)
{
    unchecked
    {
        fixed (byte* b = &buffer[start])
        {
            byte* offset = b;

            $serialize:fields
        }
    }
}";

    private const string FieldTemplate = 
@"$serialize:header:field

$serialize:value";

    private const string OptionalFieldTemplate =
@"if ($field:name != null)
{
    $serialize:header:field

    *((byte*)offset) = (byte)1;
    offset += 1;

    $serialize:value
}";

    private const string ArrayFieldTemplate = 
@"$serialize:header:field

$serialize:header:length

for (int i = 0; i < $field:name?.Length; i++)
{
    $serialize:value
}";

    private const string OptionalArrayFieldTemplate =
@"if ($field:name != null)
{
    $serialize:header:field

    *((byte*)offset) = (byte)1;
    offset += 1;

    $serialize:header:length

    for (int i = 0; i < $field:name?.Length; i++)
    {
        $serialize:value
    }
}";

    private const string StringTemplate =
@"$serialize:header:field

if ($field:name != null)
{
    $serialize:header:length

    for (int i = 0; i < $field:name.Length; i++)
    {
        *((char*)offset) = BitConverter.IsLittleEndian ? $field:accessor[i] : (char)BinaryPrimitives.ReverseEndianness($field:accessor[i]);
        offset += 2;
    }
}
else
{
    *((ushort*)offset) = (ushort)0;
    offset += 2;
}";

    private const string OptionalStringTemplate =
@"if ($field:name != null)
{
    $serialize:header:field
    
    *((byte*)offset) = (byte)1;
    offset += 1;

    $serialize:header:length

    for (int i = 0; i < $field:accessor?.Length; i++)
    {
        *((char*)offset) = BitConverter.IsLittleEndian ? $field:accessor[i] : (char)BinaryPrimitives.ReverseEndianness($field:accessor[i]);
        offset += 2;
    }
}";

    private const string StringArrayTemplate = 
@"$serialize:header:field

$serialize:header:length

for (int i = 0; i < $field:name?.Length; i++)
{
    string item = $field:name[i];

    *((ushort*)offset) = BitConverter.IsLittleEndian ? (ushort)(item?.Length ?? 0) : BinaryPrimitives.ReverseEndianness((ushort)(item?.Length ?? 0));
    offset += 2;

    if (item != null)
    {
        for (int n = 0; n < item.Length; n++)
        {
            *((char*)offset) = BitConverter.IsLittleEndian ? item[n] : (char)BinaryPrimitives.ReverseEndianness(item[n]);
            offset += 2;
        }
    }
}";

    private const string OptionalStringArrayTemplate =
@"if ($field:name != null)
{
    $serialize:header:field
    
    *((byte*)offset) = (byte)1;
    offset += 1;

    $serialize:header:length

    for (int i = 0; i < $field:name?.Length; i++)
    {
        string item = $field:name[i];

        *((ushort*)offset) = BitConverter.IsLittleEndian ? (ushort)(item?.Length ?? 0) : BinaryPrimitives.ReverseEndianness((ushort)(item?.Length ?? 0));
        offset += 2;

        if (item != null)
        {
            for (int n = 0; n < item.Length; n++)
            {
                *((char*)offset) = BitConverter.IsLittleEndian ? item[n] : (char)BinaryPrimitives.ReverseEndianness(item[n]);
                offset += 2;
            }
        }
    }
}";

    private const string FieldHeaderTemplate = 
@"*((ushort*)offset) = BitConverter.IsLittleEndian ? $field:name_ID : BinaryPrimitives.ReverseEndianness($field:name_ID);
offset += 2;";

    private const string LengthHeaderTemplate =
@"*((ushort*)offset) = BitConverter.IsLittleEndian ? (ushort)($field:name?.Length ?? 0) : BinaryPrimitives.ReverseEndianness((ushort)($field:name?.Length ?? 0));
offset += 2;";

    private const string DefaultValueTemplate = 
@"*(($field:type*)offset) = BitConverter.IsLittleEndian ? $field:accessor : BinaryPrimitives.ReverseEndianness($field:accessor);
offset += $field:size;";
    
    private const string FloatValueTemplate =
@"float g__$field:name_Copy = $field:accessor;
*((float*)offset) = BitConverter.IsLittleEndian ? $field:accessor : BinaryPrimitives.ReverseEndianness(*(uint*)&g__$field:name_Copy);
offset += 4;";

    private const string DoubleValueTemplate =
@"double g__$field:name_Copy = $field:accessor;
*((double*)offset) = BitConverter.IsLittleEndian ? $field:accessor : BinaryPrimitives.ReverseEndianness(*(ulong*)&g__$field:name_Copy);
offset += 8;";

    private const string BoolValueTemplate = 
@"*((bool*)offset) = $field:accessor;
offset += $field:size;";

    private const string ObjectValueTemplate =
@"$field:accessor.SerializeInto(buffer, (int)(offset - b));
offset += $field:accessor.GetSize();";

    private const string EnumValueTemplate =
@"*((int*)offset) = BitConverter.IsLittleEndian ? (int)$field:accessor : BinaryPrimitives.ReverseEndianness((int)$field:accessor);
offset += 4;";

    public bool CanCompile(TypeDefinition typeDefinition)
    {
        return typeDefinition.Keyword == Nsd1MessageCompiler.Keyword;
    }

    public StringBuilder Compile(TypeDefinition typeDefinition)
    {
        StringBuilder fieldsBuilder = new();
        foreach (FieldDefinition field in typeDefinition.FieldDefinitions)
        {
            StringBuilder fieldBuilder = CompileFieldSerializeInto(field);
            fieldBuilder.Insert(0, Nsd1Compiler.Indent);
            fieldBuilder.Replace("\n", "\n" + Nsd1Compiler.Indent);

            fieldsBuilder.Append(fieldBuilder);
            fieldsBuilder.AppendLine();
        }

        fieldsBuilder.Insert(0, Nsd1Compiler.Indent);
        fieldsBuilder.Replace("\n", "\n" + Nsd1Compiler.Indent);

        string serializeInto = SerializeIntoTemplate.Replace("$serialize:fields", fieldsBuilder.ToString());

        StringBuilder builder = new();
        builder.AppendLine(SerializeTemplate);
        builder.AppendLine();
        builder.AppendLine(serializeInto);
        return builder;
    }

    private StringBuilder CompileFieldSerializeInto(FieldDefinition field)
    {
        StringBuilder builder = new();

        builder.AppendLine("#region $field:name");

        if (field.TypeName != "string")
        {
            if (!field.IsOptional && !field.IsArray)
            {
                builder.AppendLine(FieldTemplate);
            }
            else if (!field.IsOptional && field.IsArray)
            {
                builder.AppendLine(ArrayFieldTemplate);
            }
            else if (field.IsOptional && !field.IsArray)
            {
                builder.AppendLine(OptionalFieldTemplate);
            }
            else if (field.IsOptional && field.IsArray)
            {
                builder.AppendLine(OptionalArrayFieldTemplate);
            }
        }
        else
        {
            if (!field.IsOptional && !field.IsArray)
            {
                builder.AppendLine(StringTemplate);
            }
            else if (!field.IsOptional && field.IsArray)
            {
                builder.AppendLine(StringArrayTemplate);
            }
            else if (field.IsOptional && !field.IsArray)
            {
                builder.AppendLine(OptionalStringTemplate);
            }
            else if (field.IsOptional && field.IsArray)
            {
                builder.AppendLine(OptionalStringArrayTemplate);
            }
        }

        builder.AppendLine("#endregion");

        builder.Replace("$serialize:header:field", FieldHeaderTemplate);
        builder.Replace("$serialize:header:length", LengthHeaderTemplate);
        builder.Replace("$serialize:value", GetFieldSerializeValueTemplate(field));

        builder.Replace("$field:name", field.Name);
        builder.Replace("$field:accessor", GetFieldAccessor(field));
        builder.Replace("$field:type", field.TypeName);
        builder.Replace("$field:size", SizeOfPrimitive(field).ToString());

        return builder;
    }

    private string GetFieldAccessor(FieldDefinition field)
    {
        string accessor = field.IsArray ? $"{field.Name}[i]" : field.Name;
        if (field.IsOptional && !field.IsArray && field.TypeName != "string")
        {
            accessor += ".Value";
        }

        return accessor;
    }

    private string GetFieldSerializeValueTemplate(FieldDefinition field)
    {
        switch (field.Type)
        {
            case FieldType.Object:
                return ObjectValueTemplate;
            case FieldType.Enum:
                return EnumValueTemplate;
            case FieldType.Primitive:
                switch (field.TypeName)
                {
                    case "bool":
                        return BoolValueTemplate;
                    case "float":
                        return FloatValueTemplate;
                    case "double":
                        return DoubleValueTemplate;
                    default:
                        return DefaultValueTemplate;
                }
            default:
                throw new NotSupportedException($"{field.TypeName} serialization is not supported.");
        }
    }

    private int SizeOfPrimitive(FieldDefinition field)
    {
        return field.TypeName switch
        {
            "bool" or "byte" or "sbyte" => 1,
            "short" or "ushort" or "char" => 2,
            "int" or "uint" => 4,
            "long" or "ulong" => 8,
            _ => -1
        };
    }
}