namespace Orpg.Shared.Services;

public interface IMessageHandler<TIdentifier, TData>
{
    TIdentifier MessageType { get; }

    void Handle(TData data);
}