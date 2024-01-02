using Needlefish.Compiler.Tests.Schema;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal interface INsdSerializerCompiler
{
    StringBuilder Compile(TypeDefinition typeDefinition);
}