using Orpg.Shared.Models;
using Orpg.Shared.Responses;

namespace Orpg.Client.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> AuthenticateAsync(BasicAuthentication credentials);

    Task<LoginResponse> LoginAsync(string authenticationToken);

    Task<LogoutResponse> LogoutAsync(string sessionToken);
}