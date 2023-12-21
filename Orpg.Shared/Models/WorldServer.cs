using System.Net;

namespace Orpg.Shared.Models;

public readonly struct WorldServer
{
    public readonly int Uid;
    public readonly string Name;
    public readonly string Status;
    public readonly IPEndPoint EndPoint;

    public WorldServer(int uid, string name, string status, IPEndPoint endPoint)
    {
        Uid = uid;
        Name = name;
        Status = status;
        EndPoint = endPoint;
    }
}