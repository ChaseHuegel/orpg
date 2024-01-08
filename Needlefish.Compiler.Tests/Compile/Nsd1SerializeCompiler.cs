﻿using Needlefish.Compiler.Tests.Schema;
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
            StringBuilder fieldBuilder = CompileFieldSerialization(field);

            fieldBuilder.Insert(0, Nsd1Compiler.Indent);
            fieldBuilder.Replace("\n", "\n" + Nsd1Compiler.Indent);
            builder.Append(fieldBuilder);
            builder.AppendLine();
        }

        builder.AppendLine("}");
        builder.AppendLine();
        return builder;
    }

    private static StringBuilder CompileFieldSerialization(FieldDefinition field)
    {
        StringBuilder builder = new();

        string hasValueStr = field.IsOptional ? $"{field.Name} != null" : "true";
        string arrayLengthStr = field.IsArray ? $"{field.Name}?.Length ?? 0" : "0";
        builder.AppendLine($"// {field.Name}");
        builder.AppendLine($"NeedlefishFormatter.WriteHeader(buffer, ref offset, {field.Name}_ID, isOptional: {field.IsOptional.ToString().ToLower()}, hasValue: {hasValueStr}, isArray: {field.IsArray.ToString().ToLower()}, arrayLength: {arrayLengthStr});");

        if (field.IsArray)
        {
            builder.AppendLine($"for (int i = 0; i < {field.Name}?.Length; i++)");
            builder.AppendLine("{");
        }
        else if (field.IsOptional)
        {
            builder.AppendLine($"if ({field.Name} != null)");
            builder.AppendLine("{");
        }

        StringBuilder fieldBuilder = CompileFieldWrite(field);
        if (field.IsArray || field.IsOptional)
        {
            fieldBuilder.Insert(0, Nsd1Compiler.Indent);
            fieldBuilder.Replace("\n", "\n" + Nsd1Compiler.Indent);
        }

        builder.Append(fieldBuilder);
        builder.AppendLine();

        if (field.IsArray || field.IsOptional)
        {
            builder.AppendLine("}");
        }

        return builder;
    }

    private static StringBuilder CompileFieldWrite(FieldDefinition field)
    {
        StringBuilder builder = new();

        string fieldAccessor = field.IsArray ? $"{field.Name}[i]" : field.Name;

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