using Needlefish.Compiler.Tests.Schema;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal class Nsd1SerializerCompiler : INsdSerializerCompiler
{
    public StringBuilder Compile(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();
        
        var serializeMethodBuilder = BuildSerializeMethod(typeDefinition);
        var deserializeMethodBuilder = BuildDeserializeMethod(typeDefinition);

        builder.Append(serializeMethodBuilder);
        builder.AppendLine();
        builder.Append(deserializeMethodBuilder);

        return builder;
    }

    private StringBuilder BuildSerializeMethod(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();

        builder.AppendLine("public byte[] Serialize()");
        builder.AppendLine("{");

        builder.AppendLine($"{Nsd1Compiler.Indent}throw new System.NotImplementException();");

        builder.AppendLine("}");
        return builder;
    }

    private StringBuilder BuildDeserializeMethod(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();

        builder.AppendLine($"public static {typeDefinition.Name} Deserialize(byte[] buffer)");
        builder.AppendLine("{");
        builder.AppendLine($"{Nsd1Compiler.Indent}{typeDefinition.Name} value = new {typeDefinition.Name}();");
        builder.AppendLine($"{Nsd1Compiler.Indent}value.Deserialize(buffer);");
        builder.AppendLine($"{Nsd1Compiler.Indent}return value;");
        builder.AppendLine("}");

        builder.AppendLine();

        builder.AppendLine("public void Deserialize(byte[] buffer)");
        builder.AppendLine("{");

        builder.AppendLine($"{Nsd1Compiler.Indent}throw new System.NotImplementException();");

        builder.AppendLine("}");
        return builder;
    }
}