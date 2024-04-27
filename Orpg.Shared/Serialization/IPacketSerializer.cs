namespace Orpg.Shared.Serialization;

public interface IPacketSerializer<T> : ISerializer<T>
{
    ushort PacketId { get; }
}