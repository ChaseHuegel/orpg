using Orpg.Client.Tests;
using Orpg.Shared.Models;

namespace Orpg.Client.Services;

public interface IAuthenticationService
{
    Task<TokenResponse> RequestTokenAsync(BasicAuthentication credentials);

    Task<SessionResponse> RequestSessionAsync(string token);
}