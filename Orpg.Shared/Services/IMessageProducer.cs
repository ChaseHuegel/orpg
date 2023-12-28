namespace Orpg.Shared.Services;

public interface IMessageProducer<T>
{
    event EventHandler<T>? NewMessage;
}
