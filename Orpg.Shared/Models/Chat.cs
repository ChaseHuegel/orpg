namespace Orpg.Shared.Models;

public readonly struct Chat
{
    public readonly int ChannelUid;
    public readonly int SenderUid;
    public readonly string Message;

    public Chat(int channelUid, int senderUid, string message)
    {
        ChannelUid = channelUid;
        SenderUid = senderUid;
        Message = message;
    }

    public override string ToString()
    {
        return $"[{ChannelUid}] {SenderUid}: {Message}";
    }
}