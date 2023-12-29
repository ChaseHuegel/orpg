using System;
using System.Collections.Generic;
using System.Linq;

namespace Needlefish.Compiler.Tests;

internal class NsdLinter
{
    private static void ValidateVersion(in List<LintIssue> issues, List<Define> _defines)
    {
        Define versionDefinition = _defines.FirstOrDefault(define => define.Key == "version");
        if (!float.TryParse(versionDefinition.Value, out _))
        {
            issues.Add(new LintIssue("Version must be defined."));
        }
    }

    private static void ValidateNoValueDuplicates(in List<LintIssue> issues, TypeDefinition definition)
    {
        FieldDefinition[] duplicates = definition.FieldDefinitions.GroupBy(f => f.Value).Where(g => g.Count() > 1).SelectMany(g => g).ToArray();

        if (duplicates.Length > 0)
        {
            var exceptions = new List<Exception>();
            for (int duplicateIndex = 0; duplicateIndex < duplicates.Length; duplicateIndex++)
            {
                FieldDefinition duplicate = duplicates[duplicateIndex];
                issues.Add(new LintIssue(string.Format("Duplicate field value. Type: ({0}), Field: ({1}), Value: ({2}).", definition.Name, duplicate.Name, duplicate.Value)));
            }
        }
    }

    private static void ValidateNoFieldDuplicates(in List<LintIssue> issues, TypeDefinition definition)
    {
        FieldDefinition[] duplicates = definition.FieldDefinitions.GroupBy(f => f.Name).Where(g => g.Count() > 1).SelectMany(g => g).ToArray();

        if (duplicates.Length > 0)
        {
            var exceptions = new List<Exception>();
            for (int i = 0; i < duplicates.Length; i++)
            {
                FieldDefinition duplicate = duplicates[i];
                issues.Add(new LintIssue(string.Format("Duplicate field definition. Type: ({0}), Field: ({1}).", definition.Name, duplicate.Name)));
            }
        }
    }

    private static void ValidateEnumFieldDefinitions(in List<LintIssue> issues, TypeDefinition definition)
    {
        for (int i = 0; i < definition.FieldDefinitions.Length; i++)
        {
            FieldDefinition field = definition.FieldDefinitions[i];
            if (field.Type != null)
            {
                issues.Add(new LintIssue(string.Format("Enums may not contain typed fields. Enum: ({0}), Value: ({1}).", definition.Name, field.Name)));
            }

            if (field.IsOptional)
            {
                issues.Add(new LintIssue(string.Format("Enums fields may not be optional. Enum: ({0}), Value: ({1}).", definition.Name, field.Name)));
            }

            if (field.IsArray)
            {
                issues.Add(new LintIssue(string.Format("Enums fields may not be arrays. Enum: ({0}), Value: ({1}).", definition.Name, field.Name)));
            }
        }
    }

    private static void ValidateEnumDefinitions(in List<LintIssue> issues, List<TypeDefinition> typeDefinitions)
    {
        foreach (TypeDefinition definition in typeDefinitions.Where(def => def.Keyword == "enum"))
        {
            ValidateNoFieldDuplicates(issues, definition);
            ValidateNoValueDuplicates(issues, definition);
            ValidateEnumFieldDefinitions(issues, definition);
        }
    }

    private static void ValidateMessageDefinitions(in List<LintIssue> issues, List<TypeDefinition> typeDefinitions)
    {
        foreach (TypeDefinition definition in typeDefinitions.Where(def => def.Keyword == "Message"))
        {
            ValidateNoValueDuplicates(issues, definition);
            ValidateNoFieldDuplicates(issues, definition);
        }
    }

    public LintIssue[] Lint(List<Define> _defines, List<TypeDefinition> _typeDefinitions)
    {
        var issues = new List<LintIssue>();

        ValidateVersion(in issues, _defines);
        ValidateMessageDefinitions(in issues, _typeDefinitions);
        ValidateEnumDefinitions(in issues, _typeDefinitions);

        return issues.ToArray();
    }
}