namespace Orpg.Shared.Models;

public readonly struct SessionResponse
{
    public readonly bool Success;
    public readonly string Message;
    public readonly int Uid;

    public SessionResponse(bool success, string message, int uid)
    {
        Success = success;
        Message = message;
        Uid = uid;
    }
}