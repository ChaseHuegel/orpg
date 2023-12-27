namespace Orpg.Shared.Services;

public readonly struct DataEventArgs
{
    public readonly byte[] Data;

    public DataEventArgs(byte[] bytes)
    {
        Data = bytes;
    }
}