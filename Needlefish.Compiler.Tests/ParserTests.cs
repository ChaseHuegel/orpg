using NUnit.Framework;

namespace Needlefish.Compiler.Tests;

public class ParserTests
{
    [Test]
    public void Parse()
    {
        Lexer<TokenType> lexer = LexerTests.LexerFactory;
        var tokens = lexer.Lex(LexerTests.ValidNsd);
        var parser = new NsdParser();
        Nsd nsd = parser.Parse(tokens);
    }

    [Test]
    public void ParseNoWhitespace()
    {
        Lexer<TokenType> lexer = LexerTests.LexerFactory;
        var tokens = lexer.Lex(LexerTests.ValidNsdNoWhitespace);
        var parser = new NsdParser();
        Nsd nsd = parser.Parse(tokens);
    }
}