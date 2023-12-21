using Orpg.Shared.Models;

namespace Orpg.Client.Tests;

public interface IWorldService
{
    Task<WorldJoinResponse> RequestJoinAsync(WorldServer world, string token, int characterUid);
    Task<WorldListResponse> RequestWorldListAsync();
}