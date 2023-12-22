namespace Orpg.Shared.Responses;

public readonly struct ClusterJoinResponse
{
    public readonly bool Success;
    public readonly string Message;

    public ClusterJoinResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}