using Orpg.Shared.Models;
using Orpg.Shared.Responses;

namespace Orpg.Client.Services;

public interface IAccountService
{
    Task<RegistrationResponse> RequestRegistrationAsync(BasicAuthentication credentials, string email);

    Task<CharacterListResponse> RequestCharacterListAsync(string authenticationToken);

    Task<CharacterDeletionResponse> RequestCharacterDeletion(string authenticationToken, int uid);

    Task<CharacterCreationResponse> RequestCharacterCreation(string authenticationToken, string name, byte archetypeId, byte raceId);
}