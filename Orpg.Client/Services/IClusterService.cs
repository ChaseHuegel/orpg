using Orpg.Shared.Models;

namespace Orpg.Client.Services;

public interface IClusterService
{
    Task<ClusterJoinResponse> RequestJoinAsync(ClusterServer world, string token);

    Task<ClusterListResponse> RequestServerListAsync();
}