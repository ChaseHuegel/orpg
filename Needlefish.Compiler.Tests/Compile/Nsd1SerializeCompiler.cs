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
        builder.AppendLine($"{Nsd1Compiler.Indent}throw new System.NotImplementedException();");
        builder.AppendLine("}");

        return builder;
    }
}