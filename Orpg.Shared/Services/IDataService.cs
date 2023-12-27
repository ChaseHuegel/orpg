namespace Orpg.Shared.Services;

public interface IDataService
{
    event EventHandler<DataEventArgs>? Received;
}