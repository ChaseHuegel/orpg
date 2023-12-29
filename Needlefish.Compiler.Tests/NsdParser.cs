using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace Needlefish.Compiler.Tests;

internal class NsdParser
{
    private static readonly TokenType[] FieldTokenTypes = new[]
    {
        TokenType.Int,
        TokenType.Uint,
        TokenType.String,
        TokenType.Float,
        TokenType.Double,
        TokenType.Long,
        TokenType.Ulong,
        TokenType.Short,
        TokenType.UShort,
        TokenType.Bool,
        TokenType.Byte,
        TokenType.Identifier,
    };

    private readonly Stack<Token<TokenType>> _tokenStack = new();
    private Token<TokenType> _lookaheadFirst;
    private Token<TokenType> _lookaheadSecond;

    private float _version;
    private readonly List<Define> _defines = new();
    private readonly List<TypeDefinition> _typeDefinitions = new();

    private void LoadSequenceStack(List<Token<TokenType>> tokens)
    {
        _tokenStack.Clear();

        int count = tokens.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            _tokenStack.Push(tokens[i]);
        }
    }

    private void PrepareLookaheads()
    {
        _lookaheadFirst = _tokenStack.Pop();
        _lookaheadSecond = _tokenStack.Pop();
    }

    private bool TryReadToken(TokenType tokenType, out Token<TokenType> token)
    {
        if (_lookaheadFirst.Type != tokenType)
        {
            token = default;
            return false;
        }

        token = _lookaheadFirst;
        DiscardToken();
        return true;
    }

    private Token<TokenType> ReadToken(TokenType tokenType)
    {
        if (!TryReadToken(tokenType, out Token<TokenType> token))
        {
            throw new FormatException(string.Format("Expected ({0}) but found ({1}).", tokenType.ToString(), _lookaheadFirst.Value));
        }

        return token;
    }

    private void DiscardToken()
    {
        _lookaheadFirst = _lookaheadSecond.Clone();

        if (_tokenStack.Any())
        {
            _lookaheadSecond = _tokenStack.Pop();
        }
        else
        {
            _lookaheadSecond = new Token<TokenType>(TokenType.Terminate, string.Empty);
        }
    }

    private bool TryDiscardToken(TokenType tokenType)
    {
        if (_lookaheadFirst.Type != tokenType)
        {
            return false;
        }

        DiscardToken();
        return true;
    }

    private void DiscardToken(TokenType tokenType)
    {
        if (!TryDiscardToken(tokenType))
        {
            throw new FormatException(string.Format("Expected ({0}) but found ({1}).", tokenType.ToString(), _lookaheadFirst.Value));
        }
    }

    private void Terminate()
    {
        TryDiscardToken(TokenType.Whitespace);
        DiscardToken(TokenType.Terminate);
    }

    private void OpenBrace()
    {
        TryDiscardToken(TokenType.Whitespace);
        DiscardToken(TokenType.OpenBrace);
    }

    private void CloseBrace()
    {
        TryDiscardToken(TokenType.Whitespace);
        DiscardToken(TokenType.CloseBrace);
    }

    private bool TryCloseBrace()
    {
        TryDiscardToken(TokenType.Whitespace);
        return TryDiscardToken(TokenType.CloseBrace);
    }

    private Token<TokenType> ReadNameIdentifier()
    {
        Token<TokenType> identifier = ReadToken(TokenType.Identifier);
        if (identifier.Value.Contains('.'))
        {
            throw new FormatException(string.Format("Expected a name but found a namespace: \"{0}\".", identifier.Value));
        }

        return identifier;
    }

    /// <summary>
    ///     Syntax: enum identifier{values...}
    /// </summary>
    private bool TryCollectEnumDefinition()
    {
        if (!TryReadToken(TokenType.Enum, out Token<TokenType> keyword))
        {
            return false;
        }

        DiscardToken(TokenType.Whitespace);
        Token<TokenType> identifier = ReadNameIdentifier();
        OpenBrace();

        var valueDefinitions = new List<FieldDefinition>();
        bool closed = false;
        while (true)
        {
            if (TryCloseBrace())
            {
                closed = true;
                break;
            }

            if (TryReadEnumValueDefinition(out FieldDefinition valueDefinition))
            {
                valueDefinitions.Add(valueDefinition);
            }
            else
            {
                break;
            }
        }

        if (!closed)
        {
            CloseBrace();
        }

        var typeDefinition = new TypeDefinition(keyword.Value, identifier.Value, valueDefinitions.ToArray());
        _typeDefinitions.Add(typeDefinition);
        return true;
    }

    /// <summary>
    ///     Syntax: identifier; OR identifier=value;
    /// </summary>
    private bool TryReadEnumValueDefinition(out FieldDefinition fieldDefinition)
    {
        TryDiscardToken(TokenType.Whitespace);
        Token<TokenType> identifier = ReadNameIdentifier();

        TryDiscardToken(TokenType.Whitespace);

        int? value = null;
        if (TryReadToken(TokenType.Equals, out _))
        {
            TryDiscardToken(TokenType.Whitespace);
            Token<TokenType> assignment = ReadToken(TokenType.Number);
            if (int.TryParse(assignment.Value, out int parsedValue))
            {
                value = parsedValue;
            }
            else
            {
                throw new FormatException(string.Format("Enum values must be integers ranging {0} to {1} ({2}).", int.MinValue, int.MaxValue, identifier.Value));
            }
        }

        Terminate();

        fieldDefinition = new FieldDefinition(null, identifier.Value, value, false, false);
        return true;
    }

    /// <summary>
    ///     Syntax: message identifier{} OR message identifier{fields...}
    /// </summary>
    private bool TryCollectMessageDefinition()
    {
        if (!TryReadToken(TokenType.Message, out Token<TokenType> keyword))
        {
            return false;
        }

        DiscardToken(TokenType.Whitespace);
        Token<TokenType> identifier = ReadNameIdentifier();
        OpenBrace();

        var fieldDefinitions = new List<FieldDefinition>();
        bool closed = false;
        while (true)
        {
            if (TryCloseBrace())
            {
                closed = true;
                break;
            }

            if (TryReadFieldDefinition(out FieldDefinition fieldDefinition))
            {
                fieldDefinitions.Add(fieldDefinition);
            }
            else
            {
                break;
            }
        }

        if (!closed)
        {
            CloseBrace();
        }

        var typeDefinition = new TypeDefinition(keyword.Value, identifier.Value, fieldDefinitions.ToArray());
        _typeDefinitions.Add(typeDefinition);
        return true;
    }

    /// <summary>
    ///     Syntax: type identifier=value; OR type[] identifier=value; OR type? identifier=value; OR type[]? identifier=value;
    /// </summary>
    private bool TryReadFieldDefinition(out FieldDefinition fieldDefinition)
    {
        TryDiscardToken(TokenType.Whitespace);
        Token<TokenType> type = ReadFieldType();

        bool hasWhitespace = false;
        hasWhitespace |= TryDiscardToken(TokenType.Whitespace);
        bool isArray = TryReadToken(TokenType.Array, out _);

        hasWhitespace |= TryDiscardToken(TokenType.Whitespace);
        bool isOptional = TryReadToken(TokenType.Optional, out _);

        hasWhitespace |= TryDiscardToken(TokenType.Whitespace);
        if (!hasWhitespace)
        {
            throw new FormatException(string.Format("Expected ({0}) but found ({1}).", TokenType.Whitespace, _lookaheadFirst.Value));
        }

        Token<TokenType> identifier = ReadNameIdentifier();
        TryDiscardToken(TokenType.Whitespace);

        ushort? id = null;
        if (TryReadToken(TokenType.Equals, out _))
        {
            TryDiscardToken(TokenType.Whitespace);
            Token<TokenType> assignment = ReadToken(TokenType.Number);
            if (ushort.TryParse(assignment.Value, out ushort parsedID))
            {
                id = parsedID;
            }
            else
            {
                throw new FormatException(string.Format("Field ({2}) must be an integer ranging {0} to {1}.", ushort.MinValue, ushort.MaxValue, identifier.Value));
            }
        }

        Terminate();

        fieldDefinition = new FieldDefinition(type.Value, identifier.Value, id, isOptional, isArray);
        return true;
    }

    private Token<TokenType> ReadFieldType()
    {
        Token<TokenType> fieldType;
        for (int i = 0; i < FieldTokenTypes.Length; i++)
        {
            TokenType fieldTokenType = FieldTokenTypes[i];
            if (TryReadToken(fieldTokenType, out fieldType))
            {
                return fieldType;
            }
        }

        throw new FormatException(string.Format("Unknown field type \"{0}\".", _lookaheadFirst.Value));
    }

    /// <summary>
    ///     Syntax: #identifier "text"|0.0|name.space; OR #identifier;
    /// </summary>
    private bool TryCollectDefine()
    {
        if (!TryReadToken(TokenType.Define, out _))
        {
            return false;
        }

        Token<TokenType> identifier = ReadNameIdentifier();
        Token<TokenType> value = ReadDefineValue(identifier);
        Terminate();

        var define = new Define(identifier.Value, value.Value);
        _defines.Add(define);
        return true;
    }

    private Token<TokenType> ReadDefineValue(Token<TokenType> identifier)
    {
        switch (identifier.Value)
        {
            case "version":
                DiscardToken(TokenType.Whitespace);
                return ReadToken(TokenType.Number);

            case "namespace":
                DiscardToken(TokenType.Whitespace);
                return ReadToken(TokenType.Identifier);

            case "include":
                DiscardToken(TokenType.Whitespace);
                var includeValue = ReadToken(TokenType.StringValue);
                return new Token<TokenType>(TokenType.StringValue, includeValue.Value.Trim('"'));

            default:
                throw new FormatException(string.Format("Unknown identifier \"{0}\".", identifier.Value));
        }
    }

    private void ValidateVersion()
    {
        Define versionDefinition = _defines.FirstOrDefault(define => define.Key == "version");
        if (!float.TryParse(versionDefinition.Value, out _version) || _version == default)
        {
            throw new FormatException("Version must be defined.");
        }
    }

    private static void ValidateNoValueDuplicates(TypeDefinition definition)
    {
        FieldDefinition[] duplicates = definition.FieldDefinitions.GroupBy(f => f.Value).Where(g => g.Count() > 1).SelectMany(g => g).ToArray();

        if (duplicates.Length > 0)
        {
            var exceptions = new List<Exception>();
            for (int duplicateIndex = 0; duplicateIndex < duplicates.Length; duplicateIndex++)
            {
                FieldDefinition duplicate = duplicates[duplicateIndex];
                var ex = new FormatException(string.Format("Duplicate field value. Type: ({0}), Field: ({1}), Value: ({2}).", definition.Name, duplicate.Name, duplicate.Value));
                exceptions.Add(ex);
            }

            throw new AggregateException(exceptions);
        }
    }

    private static void ValidateNoFieldDuplicates(TypeDefinition definition)
    {
        FieldDefinition[] duplicates = definition.FieldDefinitions.GroupBy(f => f.Name).Where(g => g.Count() > 1).SelectMany(g => g).ToArray();

        if (duplicates.Length > 0)
        {
            var exceptions = new List<Exception>();
            for (int i = 0; i < duplicates.Length; i++)
            {
                FieldDefinition duplicate = duplicates[i];
                var ex = new FormatException(string.Format("Duplicate field definition. Type: ({0}), Field: ({1}).", definition.Name, duplicate.Name));
                exceptions.Add(ex);
            }

            throw new AggregateException(exceptions);
        }
    }

    private static void ValidateEnumFieldDefinitions(TypeDefinition definition)
    {
        for (int i = 0; i < definition.FieldDefinitions.Length; i++)
        {
            FieldDefinition field = definition.FieldDefinitions[i];
            if (field.Type != null)
            {
                throw new FormatException(string.Format("Enums may not contain typed fields. Enum: ({0}), Value: ({1}).", definition.Name, field.Name));
            }

            if (field.IsOptional)
            {
                throw new FormatException(string.Format("Enums fields may not be optional. Enum: ({0}), Value: ({1}).", definition.Name, field.Name));
            }

            if (field.IsArray)
            {
                throw new FormatException(string.Format("Enums fields may not be arrays. Enum: ({0}), Value: ({1}).", definition.Name, field.Name));
            }
        }
    }

    private void ValidateEnumDefinitions()
    {
        foreach (TypeDefinition definition in _typeDefinitions.Where(def => def.Keyword == "enum"))
        {
            ValidateNoFieldDuplicates(definition);
            ValidateNoValueDuplicates(definition);
            ValidateEnumFieldDefinitions(definition);
        }
    }

    private void ValidateMessageDefinitions()
    {
        foreach (TypeDefinition definition in _typeDefinitions.Where(def => def.Keyword == "message"))
        {
            ValidateNoValueDuplicates(definition);
            ValidateNoFieldDuplicates(definition);
        }
    }

    private void ProcessTypeDefinitions()
    {
        for (int typeIndex = 0; typeIndex < _typeDefinitions.Count; typeIndex++)
        {
            TypeDefinition definition = _typeDefinitions[typeIndex];

            int currentValue = -1;
            for (int fieldIndex = 0; fieldIndex < definition.FieldDefinitions.Length; fieldIndex++)
            {
                FieldDefinition fieldDefinition = definition.FieldDefinitions[fieldIndex];
                if (fieldDefinition.Value.HasValue)
                {
                    currentValue = fieldDefinition.Value.Value;
                }
                else
                {
                    currentValue++;
                }

                definition.FieldDefinitions[fieldIndex] = new FieldDefinition(
                    fieldDefinition.Type,
                    fieldDefinition.Name,
                    currentValue,
                    fieldDefinition.IsOptional,
                    fieldDefinition.IsArray
                );
            }
        }
    }

    private void Validate()
    {
        ProcessTypeDefinitions();
        ValidateMessageDefinitions();
        ValidateEnumDefinitions();
        ValidateVersion();
    }

    public Nsd Parse(List<Token<TokenType>> tokens)
    {
        _version = default;
        _defines.Clear();
        _typeDefinitions.Clear();

        LoadSequenceStack(tokens);
        PrepareLookaheads();

        while (_tokenStack.Count > 0)
        {
            TryDiscardToken(TokenType.Whitespace);
            TryCollectDefine();

            TryDiscardToken(TokenType.Whitespace);
            TryCollectMessageDefinition();

            TryDiscardToken(TokenType.Whitespace);
            TryCollectEnumDefinition();
        }

        Validate();

        return new Nsd(_version, _defines.ToArray(), _typeDefinitions.ToArray());
    }
}