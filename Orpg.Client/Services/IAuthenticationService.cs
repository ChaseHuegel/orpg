using Orpg.Shared.Models;
using Orpg.Shared.Responses;

namespace Orpg.Client.Services;

public interface IAuthenticationService
{
    Task<TokenResponse> RequestTokenAsync(BasicAuthentication credentials);

    Task<LoginResponse> RequestLoginAsync(string token);

    Task<LogoutResponse> RequestLogoutAsync(string token);
}