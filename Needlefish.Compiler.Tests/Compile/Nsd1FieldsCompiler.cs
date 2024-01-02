using Needlefish.Compiler.Tests.Schema;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal class Nsd1FieldsCompiler : INsdTypeCompiler
{
    public bool CanCompile(TypeDefinition typeDefinition)
    {
        return typeDefinition.Keyword == Nsd1MessageCompiler.Keyword;
    }

    public StringBuilder Compile(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();

        foreach (FieldDefinition fieldDefinition in typeDefinition.FieldDefinitions)
        {
            string fullyQualifiedTypeStr = $"{fieldDefinition.Type}";

            if (fieldDefinition.IsArray)
            {
                fullyQualifiedTypeStr += "[]";
            }

            if (fieldDefinition.IsOptional)
            {
                fullyQualifiedTypeStr += "?";
            }

            string nameStr = fieldDefinition.Name;

            builder.AppendLine($"public {fullyQualifiedTypeStr} {nameStr};");
        }

        return builder;
    }
}