using Orpg.Client.Tests;
using Orpg.Shared.Models;

namespace Orpg.Client.Services;

public interface IAccountService
{
    Task<RegistrationResponse> RequestRegistrationAsync(BasicAuthentication credentials);

    Task<CharacterListResponse> RequestCharacterListAsync(string token);
}