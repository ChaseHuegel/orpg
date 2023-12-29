namespace Needlefish.Compiler.Tests;

internal readonly struct LintIssue
{
    public readonly string Message;
    public readonly IssueLevel Level;

    public LintIssue(string message, IssueLevel level = IssueLevel.Error)
    {
        Message = message;
        Level = level;
    }
}