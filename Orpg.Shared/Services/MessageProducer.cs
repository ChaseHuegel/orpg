namespace Orpg.Shared.Services;

public class MessageProducer<T> : IMessageProducer<T>, IDisposable
{
    private readonly ISerializer<T> _serializer;
    private IDataProducer[]? _dataProducers;
    private bool _disposed;

    public event EventHandler<T>? NewMessage;

    public MessageProducer(ISerializer<T> serializer, IDataProducer[] dataProducers)
    {
        List<IDataProducer> matchingDataProducers = new();
        for (int i = 0; i < dataProducers.Length; i++)
        {
            IDataProducer dataProducer = dataProducers[i];
            matchingDataProducers.Add(dataProducer);
            dataProducer.Received += OnDataReceived;
        }

        _serializer = serializer;
        _dataProducers = matchingDataProducers.ToArray();
    }

    private void OnDataReceived(object? sender, DataEventArgs e)
    {
        T value = _serializer.Deserialize(e.Data);
        NewMessage?.Invoke(this, value);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing && _dataProducers != null)
        {
            for (int i = 0; i < _dataProducers.Length; i++)
            {
                IDataProducer dataProducer = _dataProducers[i];
                dataProducer.Received -= OnDataReceived;
            }

            _dataProducers = null;
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
