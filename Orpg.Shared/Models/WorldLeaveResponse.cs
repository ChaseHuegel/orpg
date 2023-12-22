namespace Orpg.Shared.Models;

public readonly struct WorldLeaveResponse
{
    public readonly bool Success;
    public readonly string Message;

    public WorldLeaveResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}