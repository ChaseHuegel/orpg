using Needlefish.Compiler.Tests.Schema;
using System.Linq;
using System.Text;

namespace Needlefish.Compiler.Tests.Compile;

internal class Nsd1Compiler : INsdCompiler
{
    internal const string Indent = "    ";

    private static readonly string[] RequiredUsings = new[]
    {
        "using System;",
        "using Needlefish;"
    };

    private readonly INsdTypeCompiler[] TypeCompilers = new INsdTypeCompiler[] {
        new Nsd1MessageCompiler(),
        new Nsd1EnumCompiler()
    };

    public float Version => 1;

    public string Compile(Nsd nsd)
    {
        StringBuilder builder = new();

        builder.AppendLine($"// Code generated using nsd version {Version}");
        
        foreach (string usingStr in RequiredUsings)
        {
            builder.AppendLine(usingStr);
        }

        builder.AppendLine();

        Define namespaceDefine = nsd.Defines.FirstOrDefault(define => define.Key == "namespace");
        bool hasNamespace = !string.IsNullOrWhiteSpace(namespaceDefine.Value);
        if (hasNamespace)
        {
            builder.AppendLine($"namespace {namespaceDefine.Value}");
            builder.AppendLine("{");
        }

        foreach (TypeDefinition typeDefinition in nsd.TypeDefinitions.OrderBy(d => d.Keyword))
        {
            foreach (INsdTypeCompiler? typeCompiler in TypeCompilers.Where(c => c.CanCompile(typeDefinition)))
            {
                StringBuilder typeBuilder = typeCompiler.Compile(typeDefinition);

                if (hasNamespace)
                {
                    typeBuilder.Insert(0, Indent);
                    typeBuilder.Replace("\n", "\n" + Indent);
                }

                builder.Append(typeBuilder);
                builder.AppendLine();
            }
        }

        if (hasNamespace)
        {
            builder.AppendLine("}");
        }

        return builder.ToString();
    }
}