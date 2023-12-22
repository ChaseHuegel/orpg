namespace Orpg.Shared.Models;

public struct ItemStack
{
    public readonly int Uid;

    public int Count;

    public ItemStack(int uid, int count = 1)
    {
        Uid = uid;
        Count = count;
    }
}