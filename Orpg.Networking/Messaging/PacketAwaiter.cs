
using System.Runtime.CompilerServices;

namespace Orpg.Networking.Messaging;

public struct PacketAwaiter<T>
{
    private readonly IMessageProducer<T> _producer;
    private readonly TaskCompletionSource<T> _taskCompletionSource;

    public PacketAwaiter(IMessageProducer<T> messageProducer)
    {
        _taskCompletionSource = new TaskCompletionSource<T>();
        _producer = messageProducer;
        _producer.NewMessage += OnNewMessage;
    }

    private void OnNewMessage(object? sender, T e)
    {
        _producer.NewMessage -= OnNewMessage;
        _taskCompletionSource.SetResult(e);
    }

    public readonly TaskAwaiter<T> GetAwaiter()
    {
        return _taskCompletionSource.Task.GetAwaiter();
    }
}