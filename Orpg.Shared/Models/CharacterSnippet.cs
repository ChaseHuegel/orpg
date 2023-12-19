namespace Orpg.Shared.Models;

public struct CharacterSnippet
{
    public int Uid { get; }
    public string Name { get; }
    public string Description { get; }
    public string Location { get; }
    public string Activity { get; }

    public CharacterSnippet(int uid, string name, string description, string location, string activity)
    {
        Uid = uid;
        Name = name;
        Description = description;
        Location = location;
        Activity = activity;
    }
}