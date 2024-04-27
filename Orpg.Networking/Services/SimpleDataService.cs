using Orpg.Networking.Events;
using Orpg.Networking.LowLevel;

namespace Orpg.Networking.Services;

public class SimpleDataService : IDataService
{
    public event EventHandler<DataEventArgs>? Received;

    public void Post(byte[] bytes)
    {
        Received?.Invoke(this, new DataEventArgs(bytes));
    }
}