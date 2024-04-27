using Orpg.Networking.Events;
using Orpg.Networking.LowLevel;
using Orpg.Shared.Data;
using Orpg.Shared.Serialization;

namespace Orpg.Networking.Messaging;

public class PacketProducer<T> : MessageProducer<T>, IDisposable
{
    public PacketProducer(IPacketSerializer<T> serializer, IDataProducer[] dataProducers)
        : base(serializer, dataProducers) { }

    protected override void OnDataReceived(object? sender, DataEventArgs e)
    {
        Packet packet = Packet.Deserialize(e.Data, 0, e.Data.Length);
        if (packet.Id != ((IPacketSerializer<T>)_serializer).PacketId)
        {
            return;
        }

        base.OnDataReceived(sender, new DataEventArgs(packet.Data));
    }
}