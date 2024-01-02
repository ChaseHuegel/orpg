using Needlefish.Compiler.Tests.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal class Nsd1ReflectionCompiler : INsdTypeCompiler
{
    private const string ConstDefs = """
        const int byteLen = 1;
        const int boolLen = 1;
        const int shortLen = 2;
        const int intLen = 4;
        const int floatLen = 4;
        const int longLen = 8;
        const int doubleLen = 8;

        const int fieldHeaderLen = shortLen;
        const int optionalHeaderLen = boolLen;
        const int optionalFieldLen = fieldHeaderLen + optionalHeaderLen;
        const int arrayHeaderLen = shortLen;
        """;

    public bool CanCompile(TypeDefinition typeDefinition)
    {
        return typeDefinition.Keyword == Nsd1MessageCompiler.Keyword;
    }

    public StringBuilder Compile(TypeDefinition typeDefinition)
    {
        StringBuilder builder = new();

        builder.AppendLine("public int GetSize()");
        builder.AppendLine("{");

        AppendHelperConsts(builder);
        builder.AppendLine();

        AppendFieldConsts(typeDefinition, builder);
        builder.AppendLine();

        AppendMinLength(typeDefinition, builder);
        builder.AppendLine();

        builder.Append(Nsd1Compiler.Indent);
        builder.AppendLine("int length = minLength;");

        //  TODO calculate final length

        builder.Append(Nsd1Compiler.Indent);
        builder.AppendLine("return length;");

        builder.AppendLine("}");
        return builder;
    }

    private static void AppendHelperConsts(StringBuilder builder)
    {
        builder.Append(Nsd1Compiler.Indent);
        builder.AppendLine(ConstDefs.Replace("\n", "\n" + Nsd1Compiler.Indent));
    }

    private void AppendFieldConsts(TypeDefinition typeDefinition, StringBuilder builder)
    {
        foreach (FieldDefinition fieldDefinition in GetMinLenEligableFields(typeDefinition))
        {
            string fieldMinLenConst = GetFieldMinLenStr(fieldDefinition.Name, GetFieldMinLenValue(fieldDefinition));

            builder.Append(Nsd1Compiler.Indent);
            builder.AppendLine(fieldMinLenConst);
        }
    }

    private static void AppendMinLength(TypeDefinition typeDefinition, StringBuilder builder)
    {
        builder.Append(Nsd1Compiler.Indent);
        builder.Append($"const int minLength = ");

        IEnumerable<string> minLenConsts = GetMinLenEligableFields(typeDefinition).Select(f => $"{f.Name}_MinLen");

        string minLengthValue = string.Join("\n" + Nsd1Compiler.Indent + Nsd1Compiler.Indent + "+ ", minLenConsts);

        builder.Append(minLengthValue);
        builder.AppendLine(";");
    }

    private static IEnumerable<FieldDefinition> GetMinLenEligableFields(TypeDefinition typeDefinition)
    {
        //  Only non-optional fields can be applied to minLength.
        //  Optional fields aren't serialized so their const MinLen can be implied as 0.
        return typeDefinition.FieldDefinitions.Where(f => !f.IsOptional);
    }

    private static string GetFieldMinLenStr(string name, string minLen)
    {
        return string.Format("const int {0}_MinLen = fieldHeaderLen + {1};", name, minLen);
    }

    private string GetFieldMinLenValue(FieldDefinition fieldDefinition)
    {
        //  TODO need to be able to determine MinLen for enums and messages

        if (fieldDefinition.IsArray)
        {
            return "arrayHeaderLen";
        }

        switch (fieldDefinition.Type)
        {
            case "byte":
                return "byteLen";

            case "bool":
                return "boolLen";

            case "short":
            case "ushort":
                return "shortLen";

            case "int":
            case "uint":
                return "intLen";

            case "float":
                return "floatLen";

            case "long":
            case "ulong":
                return "longLen";

            case "double":
                return "doubleLen";

                //default:
                //    throw new NotSupportedException(string.Format("Unknown field type \"{0}\".", fieldDefinition.Type));
        }

        return "0";
    }
}