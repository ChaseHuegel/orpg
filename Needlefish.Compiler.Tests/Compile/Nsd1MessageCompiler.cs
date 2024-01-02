using Needlefish.Compiler.Tests.Schema;
using System;
using System.Reflection.Emit;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal class Nsd1MessageCompiler : INsdTypeCompiler
{
    private const string Keyword = "message";

    private readonly INsdSerializerCompiler[] SerializerCompilers = new INsdSerializerCompiler[] {
        new Nsd1SerializerCompiler()
    };

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

    private StringBuilder BuildMessage(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();
        builder.AppendLine($"public struct {typeDefinition.Name}");
        builder.AppendLine("{");

        AppendReflection(typeDefinition, builder);

        builder.AppendLine(Nsd1Compiler.Indent);

        AppendTypes(typeDefinition, builder);

        builder.AppendLine(Nsd1Compiler.Indent);
        
        AppendSerializer(typeDefinition, builder);

        builder.AppendLine("}");
        return builder;
    }

    private static void AppendReflection(TypeDefinition typeDefinition, StringBuilder builder)
    {
        foreach (FieldDefinition fieldDefinition in typeDefinition.FieldDefinitions)
        {
            builder.Append(Nsd1Compiler.Indent);
            AppendFieldReflection(builder, fieldDefinition);
            builder.AppendLine();
        }
    }

    private static void AppendTypes(TypeDefinition typeDefinition, StringBuilder builder)
    {
        foreach (FieldDefinition fieldDefinition in typeDefinition.FieldDefinitions)
        {
            builder.Append(Nsd1Compiler.Indent);
            AppendField(builder, fieldDefinition);
            builder.AppendLine();
        }
    }

    private void AppendSerializer(TypeDefinition typeDefinition, StringBuilder builder)
    {
        foreach (INsdSerializerCompiler serializerCompiler in SerializerCompilers)
        {
            StringBuilder serializerBuilder = serializerCompiler.Compile(typeDefinition);

            serializerBuilder.Insert(0, Nsd1Compiler.Indent);
            serializerBuilder.Replace("\n", "\n" + Nsd1Compiler.Indent);

            builder.Append(serializerBuilder);
            builder.AppendLine();
        }
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
