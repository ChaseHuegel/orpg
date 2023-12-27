namespace Orpg.Shared.Services;

public class MessageConsumer<T> : IMessageConsumer<T>, IDisposable
{
    private IMessageProducer<T>[]? _messageProducers;
    private bool _disposed;

    public event EventHandler<T>? NewMessage;

    public MessageConsumer(IMessageProducer<T>[] messageProducers)
    {
        _messageProducers = messageProducers;

        for (int i = 0; i < _messageProducers.Length; i++)
        {
            IMessageProducer<T> messageProducer = _messageProducers[i];
            messageProducer.NewMessage += OnNewMessage;
        }
    }

    private void OnNewMessage(object? sender, T e)
    {
        NewMessage?.Invoke(this, e);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing && _messageProducers != null)
        {
            for (int i = 0; i < _messageProducers.Length; i++)
            {
                IMessageProducer<T> messageProducer = _messageProducers[i];
                messageProducer.NewMessage -= OnNewMessage;
            }
        }

        _messageProducers = null;
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
