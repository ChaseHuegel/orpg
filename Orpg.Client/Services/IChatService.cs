using Orpg.Shared.Responses;

namespace Orpg.Client.Services;

public interface IChatService
{
    Task<ChatJoinResponse> RequestJoinAsync(int participantUid);

    Task<ChatLeaveResponse> RequestLeaveAsync(int participantUid);
}