namespace Needlefish.Compiler.Tests;

internal readonly struct Token<T>
{
    public readonly T Type;
    public readonly string Value;

    public Token(T type, string value)
    {
        Type = type;
        Value = value;
    }

    public Token<T> Clone()
    {
        return new Token<T>(Type, Value);
    }
}