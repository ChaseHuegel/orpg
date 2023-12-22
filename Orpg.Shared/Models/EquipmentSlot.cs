namespace Orpg.Shared.Models;

[Flags]
public enum EquipmentSlot
{
    //  Armor
    Head = 0,
    Shoulders = 1,
    Chest = 2,
    Hands = 3,
    Legs = 4,
    Feet = 5,

    //  Accessories
    Back = 10,
    Waist = 11,
    Trinket = 12,
    Neck = 13,
    Ring0 = 14,
    Ring1 = 15,
    Ear1 = 16,
    Ear2 = 17,
    Wrists = 18,

    //  Weapons
    MainHand = 20,
    OffHand = 21,
    Ranged = 22,

    //  Misc
    Undershirt = 25,
    Face = 26,

    Count = 32
}