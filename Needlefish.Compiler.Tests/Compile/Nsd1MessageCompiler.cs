using Needlefish.Compiler.Tests.Schema;
using System;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal class Nsd1MessageCompiler : INsdTypeCompiler
{
    private const string Keyword = "message";

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

        return BuildMessage(typeDefinition);
    }

    private static StringBuilder BuildMessage(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();
        builder.AppendLine($"public struct {typeDefinition.Name}");
        builder.AppendLine("{");

        foreach (FieldDefinition fieldDefinition in typeDefinition.FieldDefinitions)
        {
            builder.Append(Nsd1Compiler.Indent);
            AppendFieldReflection(builder, fieldDefinition);
            builder.AppendLine();
        }

        builder.AppendLine(Nsd1Compiler.Indent);

        foreach (FieldDefinition fieldDefinition in typeDefinition.FieldDefinitions)
        {
            builder.Append(Nsd1Compiler.Indent);
            AppendField(builder, fieldDefinition);
            builder.AppendLine();
        }

        builder.AppendLine("}");
        return builder;
    }

    private static void AppendFieldReflection(StringBuilder builder, FieldDefinition fieldDefinition)
    {
        string nameStr = fieldDefinition.Name;
        string idStr = fieldDefinition.Value.ToString()!;

        builder.Append($"private const ushort {nameStr}_ID = {idStr}");
    }

    private static void AppendField(StringBuilder builder, FieldDefinition fieldDefinition)
    {
        string fullyQualifiedTypeStr = $"{fieldDefinition.Type}";

        if (fieldDefinition.IsArray)
        {
            fullyQualifiedTypeStr = "[]";
        }

        if (fieldDefinition.IsOptional)
        {
            fullyQualifiedTypeStr += "?";
        }

        string nameStr = fieldDefinition.Name;

        builder.Append($"public {fullyQualifiedTypeStr} {nameStr};");
    }
}
