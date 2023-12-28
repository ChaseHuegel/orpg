namespace Needlefish.Compiler.Tests;

public readonly struct Nsd
{
    public float Version { get; }

    public Define[] Defines { get; }

    public TypeDefinition[] TypeDefinitions { get; }

    public Nsd(float version, Define[] defines, TypeDefinition[] typeDefinitions)
    {
        Version = version;
        Defines = defines;
        TypeDefinitions = typeDefinitions;
    }
}

public readonly struct Define
{
    public string Key { get; }
    
    public string Value { get; }
 
    public Define(string key, string value)
    {
        Key = key;
        Value = value;
    }
}

public readonly struct TypeDefinition
{
    public string Keyword { get; }
    
    public string Name { get; }

    public FieldDefinition[] FieldDefinitions { get; }
    
    public TypeDefinition(string keyword, string name, FieldDefinition[] fieldDefinitions)
    {
        Keyword = keyword;
        Name = name;
        FieldDefinitions = fieldDefinitions;
    }
}

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