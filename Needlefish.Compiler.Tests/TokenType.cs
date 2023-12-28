namespace Needlefish.Compiler.Tests;

public partial class LexerTests
{
    private enum TokenType
    {
        Define,
        Number,
        Terminate,
        StringValue,
        Identifier,
        Whitespace,
        Array,
        Optional,
        Equals,
        OpenBrace,
        CloseBrace,
        Message,
        Enum,
        String,
        Int,
        Float,
        Double,
        Long,
        Uint,
        Bool,
        Byte,
        Ulong,
        Short,
        UShort
    }
}