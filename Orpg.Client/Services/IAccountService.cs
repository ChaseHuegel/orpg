using Orpg.Shared.Models;

namespace Orpg.Client.Services;

public interface IAccountService
{
    Task<RegistrationResponse> RequestRegistrationAsync(BasicAuthentication credentials);

    Task<CharacterListResponse> RequestCharacterListAsync(string token);

    Task<CharacterDeletionResponse> RequestCharacterDeletion(string token, int uid);
}