namespace Orpg.Shared.Models;

public readonly struct ChatLeaveResponse
{
    public readonly bool Success;
    public readonly string Message;

    public ChatLeaveResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}