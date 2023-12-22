namespace Orpg.Shared.Models;

public struct AbilityData
{
    public readonly int Uid;
    public int Slot;

    public AbilityData(int uid, int slot)
    {
        Uid = uid;
        Slot = slot;
    }
}