using Orpg.Networking.Events;

namespace Orpg.Networking.LowLevel;

public interface IDataService
{
    event EventHandler<DataEventArgs>? Received;
}