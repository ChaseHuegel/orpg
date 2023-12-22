namespace Orpg.Shared.Models;

public struct SkillData
{
    public readonly int Uid;
    public int Level;
    public int XP;

    public SkillData(int uid, int level, int xp)
    {
        Uid = uid;
        Level = level;
        XP = xp;
    }
}