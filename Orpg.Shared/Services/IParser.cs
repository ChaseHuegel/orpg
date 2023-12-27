namespace Orpg.Shared.Services;

public interface IParser
{
    List<byte[]> Parse(byte[] data);
}
