using Needlefish.Compiler.Tests.Schema;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal interface INsdTypeCompiler
{
    bool CanCompile(TypeDefinition typeDefinition);
    StringBuilder Compile(TypeDefinition typeDefinition);
}