using System.Net;

namespace Orpg.Shared.Models;

public readonly struct ClusterServer
{
    public readonly int Uid;
    public readonly string Name;
    public readonly string Status;
    public readonly IPEndPoint EndPoint;

    public ClusterServer(int uid, string name, string status, IPEndPoint endPoint)
    {
        Uid = uid;
        Name = name;
        Status = status;
        EndPoint = endPoint;
    }
}