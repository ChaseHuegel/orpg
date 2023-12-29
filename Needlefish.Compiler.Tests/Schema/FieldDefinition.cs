namespace Needlefish.Compiler.Tests.Schema;

public readonly struct FieldDefinition
{
    public string? Type { get; }
    public string Name { get; }
    public int? Value { get; }
    public bool IsOptional { get; }
    public bool IsArray { get; }

    public FieldDefinition(string? type, string name, int? value, bool isOptional, bool isArray)
    {
        Type = type;
        Name = name;
        Value = value;
        IsOptional = isOptional;
        IsArray = isArray;
    }
}