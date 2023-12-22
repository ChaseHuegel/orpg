using Orpg.Shared.Models;
using Orpg.Shared.Responses;

namespace Orpg.Client.Services;

public interface IClusterService
{
    Task<ClusterJoinResponse> RequestJoinAsync(ClusterServer world, string sessionToken);

    Task<ClusterListResponse> RequestServerListAsync();
}