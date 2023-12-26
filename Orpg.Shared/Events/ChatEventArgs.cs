using Orpg.Shared.Models;

namespace Orpg.Shared.Events;

public readonly struct ChatEventArgs
{
    public readonly Chat Chat;

    public ChatEventArgs(Chat chat)
    {
        Chat = chat;
    }
}