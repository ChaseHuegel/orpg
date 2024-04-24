using Needlefish.Compiler.Tests.Compile;
using Needlefish.Compiler.Tests.Lexing;
using Needlefish.Compiler.Tests.Schema;
using System;
using System.Collections.Generic;

namespace Needlefish.Compiler.Tests;

public class NsdGenerator
{
    public static readonly Version Version1 = new(1, 0);

    private static readonly List<TokenDefinition<TokenType>> TokenDefinitions = new()
    {
            new(TokenType.Whitespace, @"\G[\s\t\n\r\f\0]+"),
            new(TokenType.Define, @"\G#"),
            new(TokenType.Number, @"\G[+-]?\d*\.?\d+"),
            new(TokenType.Terminate, @"\G;"),
            new(TokenType.Array, @"\G\[\]"),
            new(TokenType.Optional, @"\G\?"),
            new(TokenType.Equals, @"\G="),
            new(TokenType.OpenBrace, @"\G{"),
            new(TokenType.CloseBrace, @"\G}"),
            new(TokenType.Message, @"\Gmessage"),
            new(TokenType.Enum, @"\Genum"),
            new(TokenType.String, @"\Gstring"),
            new(TokenType.Int, @"\Gint"),
            new(TokenType.Float, @"\Gfloat"),
            new(TokenType.Double, @"\Gdouble"),
            new(TokenType.Long, @"\Glong"),
            new(TokenType.Uint, @"\Guint"),
            new(TokenType.Bool, @"\Gbool"),
            new(TokenType.Byte, @"\Gbyte"),
            new(TokenType.Ulong, @"\Gulong"),
            new(TokenType.Short, @"\Gshort"),
            new(TokenType.UShort, @"\Gushort"),
            new(TokenType.StringValue, @"\G""[^""]*"""),
            new(TokenType.Identifier, @"\G[a-zA-Z]*([.][a-zA-Z]|[a-zA-Z0-9_])+"),
    };

    private object Lock = new object();
    private Lexer<TokenType> Lexer = new(TokenDefinitions);
    private NsdParser Parser = new();
    private INsdCompiler Compiler;

    public NsdGenerator(Version version)
    {
        if (version.Equals(Version1))
        {
            Compiler = new Nsd1Compiler();
        }

        if (Compiler == null)
        {
            throw new NotSupportedException($"Unsupported version {version}.");
        }
    }

    public string Generate(string name, string source)
    {
        lock (Lock)
        {
            List<Token<TokenType>> tokens = Lexer.Lex(source);
            Nsd nsd = Parser.Parse(tokens);
            return Compiler.Compile(nsd, name);
        }
    }

    public string[] Generate(KeyValuePair<string, string>[] sources)
    {
        string[] results = new string[sources.Length];

        lock (Lock)
        {
            for (int i = 0; i < sources.Length; i++)
            {
                KeyValuePair<string, string> source = sources[i];
                List<Token<TokenType>> tokens = Lexer.Lex(source.Value);
                Nsd nsd = Parser.Parse(tokens);
                results[i] = Compiler.Compile(nsd, source.Key);
            }
        }

        return results;
    }
}
