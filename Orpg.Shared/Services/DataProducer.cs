namespace Orpg.Shared.Services;

public abstract class DataProducer : IDataProducer, IDisposable
{
    private readonly IDataService _dataService;
    private readonly IParser _parser;
    private bool _disposed;

    public event EventHandler<DataEventArgs>? Received;

    public abstract string Format { get; }

    public DataProducer(IDataService dataService, IParser parser)
    {
        _dataService = dataService;
        _parser = parser;

        _dataService.Received += OnDataReceived;
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

        if (disposing)
        {
            _dataService.Received -= OnDataReceived;
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
