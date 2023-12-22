namespace Orpg.Shared.Models;

public struct CharacterMetadata
{
    public readonly int Uid;
    public int XP;
    public readonly Inventory Equipment;
    public readonly AttributeData[] Attributes;
    public readonly SkillData[] Skills;

    public CharacterMetadata(int uid, int xp, Inventory equipment, AttributeData[] attributes, SkillData[] skills)
    {
        Uid = uid;
        XP = xp;
        Equipment = equipment;
        Attributes = attributes;
        Skills = skills;
    }
}