namespace Orpg.Shared.Models;

public struct AttributeData
{
    public readonly int Uid;
    public int Value;

    public AttributeData(int uid, int value)
    {
        Uid = uid;
        Value = value;
    }
}
