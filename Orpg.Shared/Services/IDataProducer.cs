namespace Orpg.Shared.Services;

public interface IDataProducer
{
    event EventHandler<DataEventArgs>? Received;
}
