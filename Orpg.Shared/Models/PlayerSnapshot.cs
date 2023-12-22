namespace Orpg.Shared.Models;

public readonly struct PlayerSnapshot
{
    public readonly Character Character;
    public readonly CharacterMetadata Metadata;
    public readonly Position Position;
    public readonly Inventory Inventory;
    public readonly AbilityData[] Abilities;

    public PlayerSnapshot(Character character, CharacterMetadata metadata, Position position, Inventory inventory, AbilityData[] abilities)
    {
        Character = character;
        Metadata = metadata;
        Position = position;
        Inventory = inventory;
        Abilities = abilities;
    }
}