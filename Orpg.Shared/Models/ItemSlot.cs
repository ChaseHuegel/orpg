namespace Orpg.Shared.Models;

public struct ItemSlot
{
    public readonly int Slot;

    public ItemStack ItemStack;

    public ItemSlot(int slot, ItemStack itemStack)
    {
        Slot = slot;
        ItemStack = itemStack;
    }
}