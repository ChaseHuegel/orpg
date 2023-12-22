using Orpg.Shared.Models;

namespace Orpg.Shared.Responses;

public readonly struct ChatJoinResponse
{
    public readonly bool Success;
    public readonly string Message;
    public readonly ChatChannel[] Channels;

    public ChatJoinResponse(bool success, string message, ChatChannel[] channels)
    {
        Success = success;
        Message = message;
        Channels = channels;
    }
}