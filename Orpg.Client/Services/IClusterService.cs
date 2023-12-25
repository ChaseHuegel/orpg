using Orpg.Shared.Models;
using Orpg.Shared.Responses;

namespace Orpg.Client.Services;

public interface IClusterService
{
    Task<ClusterJoinResponse> RequestJoinAsync(ClusterServer server, string sessionToken);

    Task<ClusterListResponse> RequestServerListAsync();
}