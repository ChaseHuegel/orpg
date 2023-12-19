using Orpg.Client.Tests;
using Orpg.Shared.Models;

namespace Orpg.Client.Services;

public interface IAccountService
{
    Task<RegistrationResponse> RegisterAsync(BasicAuthentication credentials);

    Task<CharacterListResponse> RequestCharacterListAsync(string token);
}