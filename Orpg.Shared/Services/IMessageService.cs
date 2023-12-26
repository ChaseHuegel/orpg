namespace Orpg.Shared.Services;

public interface IMessageService<T>
{
    void Post(int type, byte[] data);
}
