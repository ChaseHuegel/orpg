using Orpg.Shared.Models;
using Orpg.Shared.Responses;

namespace Orpg.Client.Services;

public interface IAccountService
{
    Task<RegistrationResponse> RequestRegistrationAsync(BasicAuthentication credentials, string email);

    Task<CharacterListResponse> RequestCharacterListAsync(string token);

    Task<CharacterDeletionResponse> RequestCharacterDeletion(string token, int uid);

    Task<CharacterCreationResponse> RequestCharacterCreation(string token, string name, byte archetypeId, byte raceId);
}