namespace Orpg.Shared.Models;

public readonly struct LogoutResponse
{
    public readonly bool Success;
    public readonly string Message;

    public LogoutResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}