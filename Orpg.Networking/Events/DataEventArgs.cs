namespace Orpg.Networking.Events;

public readonly struct DataEventArgs
{
    public readonly byte[] Data;

    public DataEventArgs(byte[] bytes)
    {
        Data = bytes;
    }
}