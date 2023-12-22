namespace Orpg.Shared.Models;

public readonly struct CharacterAppearance
{
    public byte HairId { get; }
    public byte FaceId { get; }
    public byte BodyId { get; }
    public byte SkinId { get; }
    public byte Size { get; }
}
