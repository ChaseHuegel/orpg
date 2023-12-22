using Orpg.Shared.Models;

namespace Orpg.Client.Tests;

public interface IClusterService
{
    Task<ClusterJoinResponse> RequestJoinAsync(ClusterServer world, string token, int characterUid);
    Task<ClusterListResponse> RequestServerListAsync();
}