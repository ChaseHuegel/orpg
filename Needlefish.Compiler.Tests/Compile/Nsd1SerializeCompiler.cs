using Needlefish.Compiler.Tests.Schema;
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
        builder.AppendLine($"{Nsd1Compiler.Indent}int offset = 0;");
        builder.AppendLine();

        foreach (FieldDefinition field in typeDefinition.FieldDefinitions)
        {
            AppendField(builder, field);
            builder.AppendLine();
        }

        builder.AppendLine($"{Nsd1Compiler.Indent}return buffer;");
        builder.AppendLine("}");
        return builder;
    }

    private static void AppendField(StringBuilder builder, FieldDefinition field)
    {
        if (!field.IsArray && !field.IsOptional)
        {
            builder.Append(Nsd1Compiler.Indent);
            builder.AppendLine($"encode_{field.TypeName}(buffer, ref offset, {field.Name}_ID, ref {field.Name});");
        }
        else if (!field.IsArray && field.IsOptional)
        {
            builder.Append(Nsd1Compiler.Indent);
            builder.AppendLine($"if ({field.Name} != null)");
            
            builder.Append(Nsd1Compiler.Indent);
            builder.AppendLine("{");

            builder.Append(Nsd1Compiler.Indent);
            builder.Append(Nsd1Compiler.Indent);
            builder.AppendLine($"encode_optional_{field.TypeName}(buffer, ref offset, {field.Name}_ID, ref {field.Name});");
            
            builder.Append(Nsd1Compiler.Indent);
            builder.AppendLine("}");
        }
        if (field.IsArray && !field.IsOptional)
        {
            builder.Append(Nsd1Compiler.Indent);
            builder.AppendLine($"encode_{field.TypeName}_array(buffer, ref offset, {field.Name}_ID, ref {field.Name});");
        }
        else if (field.IsArray && field.IsOptional)
        {
            builder.Append(Nsd1Compiler.Indent);
            builder.AppendLine($"if ({field.Name} != null)");

            builder.Append(Nsd1Compiler.Indent);
            builder.AppendLine("{");

            builder.Append(Nsd1Compiler.Indent);
            builder.Append(Nsd1Compiler.Indent);
            builder.AppendLine($"encode_optional_{field.TypeName}_array(buffer, ref offset, {field.Name}_ID, ref {field.Name});");

            builder.Append(Nsd1Compiler.Indent);
            builder.AppendLine("}");
        }
    }
}