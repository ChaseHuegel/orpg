using Needlefish.Compiler.Tests.Schema;
using System;
using System.Reflection.Emit;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal class Nsd1SerializeCompiler : INsdTypeCompiler
{
    public bool CanCompile(TypeDefinition typeDefinition)
    {
        return typeDefinition.Keyword == Nsd1MessageCompiler.Keyword;
    }

    public StringBuilder Compile(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();

        builder.AppendLine("public byte[] Serialize()");
        builder.AppendLine("{");
        builder.AppendLine($"{Nsd1Compiler.Indent}byte[] buffer = new byte[GetSize()];");
        builder.AppendLine($"{Nsd1Compiler.Indent}SerializeInto(buffer);");
        builder.AppendLine($"{Nsd1Compiler.Indent}return buffer;");
        builder.AppendLine("}");
        builder.AppendLine();

        builder.AppendLine("public void SerializeInto(byte[] buffer)");
        builder.AppendLine("{");
        builder.AppendLine($"{Nsd1Compiler.Indent}int offset = 0;");
        builder.AppendLine();

        foreach (FieldDefinition field in typeDefinition.FieldDefinitions)
        {
            if (field.IsOptional)
            {
                builder.AppendLine($"{Nsd1Compiler.Indent}if ({field.Name} != null)");
                builder.AppendLine($"{Nsd1Compiler.Indent}{{");
            }

            StringBuilder fieldBuilder = CompileFieldSerialization(field);

            string indent = field.IsOptional ? Nsd1Compiler.Indent + Nsd1Compiler.Indent : Nsd1Compiler.Indent;
            fieldBuilder.Insert(0, indent);
            fieldBuilder.Replace("\n", "\n" + indent);

            builder.Append(fieldBuilder);
            builder.AppendLine();

            if (field.IsOptional)
            {
                builder.AppendLine($"{Nsd1Compiler.Indent}}}");
                builder.AppendLine();
            }
        }

        builder.AppendLine("}");
        builder.AppendLine();
        return builder;
    }

    private static StringBuilder CompileFieldSerialization(FieldDefinition field)
    {
        StringBuilder builder = new();

        string arrayLengthStr = field.IsArray ? $"(ushort)({field.Name}?.Length ?? 0)" : "(ushort)0";

        builder.AppendLine($"#region {field.Name}");
        builder.AppendLine($"NeedlefishFormatter.WriteHeader(buffer, ref offset, {field.Name}_ID, isOptional: {field.IsOptional.ToString().ToLower()}, hasValue: true, isArray: {field.IsArray.ToString().ToLower()}, arrayLength: {arrayLengthStr});");

        if (field.IsArray)
        {
            builder.AppendLine($"for (int i = 0; i < {field.Name}?.Length; i++)");
            builder.AppendLine("{");
        }

        StringBuilder fieldBuilder = CompileFieldWrite(field);
        if (field.IsArray)
        {
            fieldBuilder.Insert(0, Nsd1Compiler.Indent);
            fieldBuilder.Replace("\n", "\n" + Nsd1Compiler.Indent);
        }

        builder.Append(fieldBuilder);
        builder.AppendLine();

        if (field.IsArray)
        {
            builder.AppendLine("}");
        }
        builder.AppendLine("#endregion");

        return builder;
    }

    private static StringBuilder CompileFieldWrite(FieldDefinition field)
    {
        StringBuilder builder = new();

        string fieldAccessor = field.IsArray ? $"{field.Name}[i]" : field.Name;
        if (field.IsOptional && !field.IsArray && field.TypeName != "string")
        {
            fieldAccessor += ".Value";
        }

        switch (field.Type)
        {
            case FieldType.Object:
                builder.Append($"{fieldAccessor}.SerializeInto(buffer);");
                break;

            case FieldType.Enum:
                builder.Append($"NeedlefishFormatter.Write(buffer, ref offset, (int){fieldAccessor});");
                break;

            case FieldType.Primitive:
                builder.Append($"NeedlefishFormatter.Write(buffer, ref offset, {fieldAccessor});");
                break;
        }

        return builder;
    }
}