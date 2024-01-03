using Lexer.Tests;
using Needlefish.Compiler.Tests.Compile;
using Needlefish.Compiler.Tests.Schema;
using NUnit.Framework;

namespace Needlefish.Compiler.Tests;

internal class CompilerTests
{
    [Test]
    public void Compile()
    {
        Nsd nsd = ParserTests.ParseNsdContent(LexerTests.ValidNsd);

        var compiler = new Nsd1Compiler();
        
        string result = compiler.Compile(nsd);

        Assert.Pass(result);
    }
}
