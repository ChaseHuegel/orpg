namespace Orpg.Networking.Messaging;

public interface IMessageConsumer<T>
{
    event EventHandler<T>? NewMessage;
}
