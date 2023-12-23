using Orpg.Shared.Models;

namespace Orpg.Client.Services;

public readonly struct ChatEventArgs
{
    public readonly Chat Chat;

    public ChatEventArgs(Chat chat)
    {
        Chat = chat;
    }
}