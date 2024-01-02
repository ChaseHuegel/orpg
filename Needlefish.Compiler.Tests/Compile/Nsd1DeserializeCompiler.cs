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
        
        var deserializeHelperBuilder = BuildDeserializeHelper(typeDefinition);
        var deserializeBuilder = BuildDeserialize(typeDefinition);

        builder.Append(deserializeHelperBuilder);
        builder.AppendLine();
        builder.Append(deserializeBuilder);

        return builder;
    }

    private static StringBuilder BuildDeserializeHelper(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();

        builder.AppendLine($"public static {typeDefinition.Name} Deserialize(byte[] buffer)");
        builder.AppendLine("{");
        builder.AppendLine($"{Nsd1Compiler.Indent}{typeDefinition.Name} value = new {typeDefinition.Name}();");
        builder.AppendLine($"{Nsd1Compiler.Indent}value.Deserialize(buffer);");
        builder.AppendLine($"{Nsd1Compiler.Indent}return value;");
        builder.AppendLine("}");

        return builder;
    }

    private static StringBuilder BuildDeserialize(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();

        builder.AppendLine("public void Deserialize(byte[] buffer)");
        builder.AppendLine("{");
        builder.AppendLine($"{Nsd1Compiler.Indent}throw new System.NotImplementException();");
        builder.AppendLine("}");

        return builder;
    }
}