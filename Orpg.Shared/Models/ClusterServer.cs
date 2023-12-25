using System.Net;

namespace Orpg.Shared.Models;

public readonly struct ClusterServer
{
    public readonly string Identity;
    public readonly string Name;
    public readonly string Status;
    public readonly int UserCount;
    public readonly IPEndPoint EndPoint;

    public ClusterServer(string identity, string name, string status, int userCount, IPEndPoint endPoint)
    {
        Identity = identity;
        Name = name;
        Status = status;
        UserCount = userCount;
        EndPoint = endPoint;
    }
}