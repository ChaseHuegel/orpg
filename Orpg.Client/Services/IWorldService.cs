using Orpg.Shared.Models;

namespace Orpg.Client.Services;

public interface IWorldService
{
    Task<WorldEntryResponse> RequestEntryAsync(int uid);

    Task<WorldLeaveResponse> RequestLeaveAsync(int uid);
}