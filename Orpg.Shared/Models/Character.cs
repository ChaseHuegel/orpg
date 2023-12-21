namespace Orpg.Shared.Models;

public readonly struct Character
{
    public int Uid { get; }
    public string Name { get; }
    public byte Level { get; }
    public byte ArchetypeId { get; }
    public byte RaceId { get; }
    public string Location { get; }
    public string Activity { get; }
    public Appearance Appearance { get; }
    public Visuals Visuals { get; }

    public Character(int uid, string name, byte level, byte archetypeId, byte raceId, string location, string activity, Appearance appearance, Visuals visuals)
    {
        Uid = uid;
        Name = name;
        Level = level;
        ArchetypeId = archetypeId;
        RaceId = raceId;
        Location = location;
        Activity = activity;
        Appearance = appearance;
        Visuals = visuals;
    }
}