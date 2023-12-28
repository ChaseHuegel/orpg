using Orpg.Networking.Events;

namespace Orpg.Networking.LowLevel;

public interface IDataProducer
{
    event EventHandler<DataEventArgs>? Received;
}
