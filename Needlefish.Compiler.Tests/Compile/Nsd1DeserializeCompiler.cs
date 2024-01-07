using Needlefish.Compiler.Tests.Schema;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal class Nsd1DeserializeCompiler : INsdTypeCompiler
{
    public bool CanCompile(TypeDefinition typeDefinition)
    {
        return typeDefinition.Keyword == Nsd1MessageCompiler.Keyword;
    }

    public StringBuilder Compile(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();
        
        var deserializeHelperBuilder = BuildDeserialize(typeDefinition);
        var deserializeBuilder = BuildUnpack(typeDefinition);

        builder.Append(deserializeHelperBuilder);
        builder.AppendLine();
        builder.Append(deserializeBuilder);

        return builder;
    }

    private static StringBuilder BuildDeserialize(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();

        builder.AppendLine($"public static {typeDefinition.Name} Deserialize(byte[] buffer)");
        builder.AppendLine("{");
        builder.AppendLine($"{Nsd1Compiler.Indent}{typeDefinition.Name} value = new {typeDefinition.Name}();");
        builder.AppendLine($"{Nsd1Compiler.Indent}value.Unpack(buffer);");
        builder.AppendLine($"{Nsd1Compiler.Indent}return value;");
        builder.AppendLine("}");

        return builder;
    }

    private static StringBuilder BuildUnpack(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();

        builder.AppendLine("public void Unpack(byte[] buffer)");
        builder.AppendLine("{");
        builder.AppendLine($"{Nsd1Compiler.Indent}int offset = 0;");
        builder.AppendLine($"{Nsd1Compiler.Indent}while (offset < buffer.Length)");
        builder.AppendLine($"{Nsd1Compiler.Indent}{{");
        builder.AppendLine($"{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}ushort id = NeedlefishFormatter.ReadUShort(buffer, ref offset);");
        builder.AppendLine($"{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}switch (id)");
        builder.AppendLine($"{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}{{");

        foreach (FieldDefinition field in typeDefinition.FieldDefinitions)
        {
            builder.AppendLine($"{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}case {field.Name}_ID:");
            builder.Append($"{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}");
            AppendField(builder, field);
            builder.AppendLine($"{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}break;");
        }

        builder.AppendLine($"{Nsd1Compiler.Indent}{Nsd1Compiler.Indent}}}");
        builder.AppendLine($"{Nsd1Compiler.Indent}}}");
        builder.AppendLine("}");

        return builder;
    }

    private static void AppendField(StringBuilder builder, FieldDefinition field)
    {
        if (!field.IsArray && !field.IsOptional)
        {
            builder.AppendLine($"decode_{field.TypeName}(buffer, ref offset, ref {field.Name});");
        }
        else if (!field.IsArray && field.IsOptional)
        {
            builder.AppendLine($"decode_optional_{field.TypeName}(buffer, ref offset, ref {field.Name});");
        }
        if (field.IsArray && !field.IsOptional)
        {
            builder.AppendLine($"decode_{field.TypeName}_array(buffer, ref offset, ref {field.Name});");
        }
        else if (field.IsArray && field.IsOptional)
        {
            builder.AppendLine($"decode_optional_{field.TypeName}_array(buffer, ref offset, ref {field.Name});");
        }
    }
}