namespace Orpg.Shared.Models;

public readonly struct ChatChannel
{
    public readonly int Uid;
    public readonly string Name;

    public ChatChannel(int uid, string name)
    {
        Uid = uid;
        Name = name;
    }
}