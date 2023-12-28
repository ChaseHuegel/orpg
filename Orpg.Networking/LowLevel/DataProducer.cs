using Orpg.Networking.Events;

namespace Orpg.Networking.LowLevel;

public class DataProducer : IDataProducer, IDisposable
{
    private IDataService[]? _dataServices;
    private readonly IParser _parser;
    private bool _disposed;

    public event EventHandler<DataEventArgs>? Received;

    public DataProducer(IDataService[] dataServices, IParser parser)
    {
        _dataServices = dataServices;
        _parser = parser;

        for (int i = 0; i < dataServices.Length; i++)
        {
            IDataService dataService = dataServices[i];
            dataService.Received += OnDataReceived;
        }
    }

    private void OnDataReceived(object? sender, DataEventArgs e)
    {
        List<byte[]> dataPackets = _parser.Parse(e.Data);

        for (int i = 0; i < dataPackets.Count; i++)
        {
            byte[] packet = dataPackets[i];
            Received?.Invoke(this, new DataEventArgs(packet));
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing && _dataServices != null)
        {
            for (int i = 0; i < _dataServices.Length; i++)
            {
                IDataService dataService = _dataServices[i];
                dataService.Received -= OnDataReceived;
            }

            _dataServices = null;
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
