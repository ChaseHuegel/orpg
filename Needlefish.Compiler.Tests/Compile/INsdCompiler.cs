using Needlefish.Compiler.Tests.Schema;

namespace Needlefish.Compiler.Tests.Compile;

internal interface INsdCompiler
{
    float Version { get; }

    string Compile(Nsd nsd);
}