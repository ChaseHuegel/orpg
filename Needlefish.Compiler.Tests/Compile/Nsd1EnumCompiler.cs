using Needlefish.Compiler.Tests.Schema;
using System;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal class Nsd1EnumCompiler : INsdTypeCompiler
{
    private const string Keyword = "enum";

    public bool CanCompile(TypeDefinition typeDefinition)
    {
        return typeDefinition.Keyword == Keyword;
    }

    public StringBuilder Compile(TypeDefinition typeDefinition)
    {
        if (typeDefinition.Keyword != Keyword)
        {
            throw new InvalidOperationException(string.Format(
                "Invalid {0}. Expected keyword \"{1}\" but got \"{2}\".",
                nameof(TypeDefinition),
                Keyword,
                typeDefinition.Keyword)
            );
        }

        return BuildEnum(typeDefinition);
    }

    private static StringBuilder BuildEnum(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();
        builder.AppendLine($"public enum {typeDefinition.Name}");
        builder.AppendLine("{");

        foreach (FieldDefinition fieldDefinition in typeDefinition.FieldDefinitions)
        {
            builder.Append(Nsd1Compiler.Indent);
            AppendValue(builder, fieldDefinition);
            builder.AppendLine();
        }

        builder.AppendLine("}");
        return builder;
    }

    private static void AppendValue(StringBuilder builder, FieldDefinition fieldDefinition)
    {
        builder.Append($"{fieldDefinition.Name} = {fieldDefinition.Value},");
    }
}
