namespace Orpg.Shared.Services;

public interface IMessageConsumer<T>
{
    event EventHandler<T>? NewMessage;
}
