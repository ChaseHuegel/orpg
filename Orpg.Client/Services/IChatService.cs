using Orpg.Shared.Responses;

namespace Orpg.Client.Services;

public interface IChatService
{
    event EventHandler<ChatEventArgs> Received;

    void StartListening();
    
    void StopListening();

    Task<ChatJoinResponse> RequestJoinAsync(int participantUid);

    Task<ChatLeaveResponse> RequestLeaveAsync(int participantUid);
}