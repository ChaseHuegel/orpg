namespace Orpg.Shared.Serialization;

public interface IParser
{
    List<byte[]> Parse(byte[] data);
}
