using Orpg.Shared.Models;
using Orpg.Shared.Responses;
using Orpg.Shared.Types;

namespace Orpg.Client.Services;

public interface IChatService
{
    event EventHandler<ChatEventArgs> Received;

    void StartListening();
    
    void StopListening();

    Task<ChatJoinResponse> RequestJoinAsync(int participantUid);

    Task<ChatLeaveResponse> RequestLeaveAsync(int participantUid);

    Task<Result> SendAsync(Chat chat);
}