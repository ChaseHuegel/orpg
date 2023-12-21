namespace Orpg.Shared.Models;

public readonly struct WorldJoinResponse
{
    public readonly bool Success;
    public readonly string Message;

    public WorldJoinResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}