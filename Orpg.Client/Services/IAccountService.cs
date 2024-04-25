using Orpg.Shared.Data;
using Orpg.Shared.Models;
using Orpg.Shared.Responses;

namespace Orpg.Client.Services;

public interface IAccountService
{
    Task<RegistrationResponse> RequestRegistrationAsync(BasicAuthentication credentials, string email);

    Task<Shared.Data.CharacterListResponse> RequestCharacterListAsync(string authenticationToken);

    Task<Shared.Data.CharacterDeletionResponse> RequestCharacterDeletion(string authenticationToken, int uid);

    Task<Shared.Data.CharacterCreationResponse> RequestCharacterCreation(string authenticationToken, string name, byte archetypeId, byte raceId);
}