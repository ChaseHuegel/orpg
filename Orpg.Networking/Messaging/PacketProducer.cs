using Orpg.Networking.Events;
using Orpg.Networking.LowLevel;
using Orpg.Shared.Serialization;

namespace Orpg.Networking.Messaging;

public class PacketProducer<T> : MessageProducer<T>, IDisposable
{
    public PacketProducer(IPacketSerializer<T> serializer, IDataProducer[] dataProducers)
        : base(serializer, dataProducers) { }

    protected override void OnDataReceived(object? sender, DataEventArgs e)
    {
        ushort packetId = BitConverter.ToUInt16(e.Data, 0);
        if (packetId != ((IPacketSerializer<T>)_serializer).PacketId)
        {
            return;
        }

        base.OnDataReceived(sender, e);
    }
}