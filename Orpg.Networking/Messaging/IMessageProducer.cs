namespace Orpg.Networking.Messaging;

public interface IMessageProducer<T>
{
    event EventHandler<T>? NewMessage;
}
