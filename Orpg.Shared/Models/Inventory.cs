namespace Orpg.Shared.Models;

public struct Inventory
{
    public readonly int Size;

    private readonly ItemStack[] _contents;

    public Inventory(int size)
    {
        Size = size;
        _contents = new ItemStack[size];
    }

    public Inventory(int size, params ItemStack[] contents) : this(size)
    {
        if (contents.Length > size)
        {
            throw new ArgumentException($"Length ({contents.Length}) is greater than {nameof(size)} ({size}).", nameof(contents));
        }

        contents.CopyTo(contents, 0);
    }

    public Inventory(int size, params ItemSlot[] contents) : this(size)
    {
        if (contents.Length > size)
        {
            throw new ArgumentException($"Length ({contents.Length}) is greater than {nameof(size)} ({size}).", nameof(contents));
        }

        _contents = new ItemStack[size];

        for (int i = 0; i < contents.Length; i++)
        {
            ItemSlot itemSlot = contents[i];
            _contents[itemSlot.Slot] = itemSlot.ItemStack;
        }
    }
}