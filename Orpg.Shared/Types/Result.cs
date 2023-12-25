namespace Orpg.Shared.Types;

public readonly struct Result
{
    public readonly bool Success;
    public readonly string Message;

    public Result(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}

public readonly struct Result<T>
{
    public readonly bool Success;
    public readonly string Message;
    public readonly T Value;

    public Result(bool success, string message, T value)
    {
        Success = success;
        Message = message;
        Value = value;
    }
}