using DryIoc;
using Moq;
using Orpg.Client.Services;
using Orpg.Shared.Models;

namespace Orpg.Client.Tests;

public class AuthenticationTests : TestBase
{
    private const string Username = "default";
    private const string Password = "password";
    private const string Token = "1234";
    private const int SessionUid = 1;

    protected override void Setup(Container container)
    {
        var mockAuthenticationService = new Mock<IAuthenticationService>();

        mockAuthenticationService.Setup(
            authenticationService => authenticationService.RequestTokenAsync(new BasicAuthentication(Username, Password))
        ).ReturnsAsync(new TokenResponse(true, $"Authenticated as \"{Username}\".", Token));

        mockAuthenticationService.Setup(
            authenticationService => authenticationService.RequestLoginAsync(Token)
        ).ReturnsAsync(new LoginResponse(true, $"Logged in as \"{Username}\"."));

        mockAuthenticationService.Setup(
            authenticationService => authenticationService.RequestLogoutAsync(Token)
        ).ReturnsAsync(new LogoutResponse(true, $"Logged out as \"{Username}\"."));

        container.RegisterInstance(mockAuthenticationService.Object);
    }

    [Test]
    public async Task Authenticate()
    {
        var authenticationService = Container.Resolve<IAuthenticationService>();

        var credentials = new BasicAuthentication(Username, Password);
        TokenResponse tokenResponse = await authenticationService.RequestTokenAsync(credentials);

        Assert.Multiple(() =>
        {
            Assert.That(tokenResponse.Success, Is.True);
            Assert.That(tokenResponse.Token, Is.Not.Null);
        });

        Console.WriteLine(tokenResponse.Message);
    }

    [Test]
    public async Task Login()
    {
        var authenticationService = Container.Resolve<IAuthenticationService>();

        var credentials = new BasicAuthentication(Username, Password);
        TokenResponse tokenResponse = await authenticationService.RequestTokenAsync(credentials);
        LoginResponse loginResponse = await authenticationService.RequestLoginAsync(tokenResponse.Token);

        Assert.Multiple(() =>
        {
            Assert.That(loginResponse.Success, Is.True);
        });

        Console.WriteLine(loginResponse.Message);
    }

    [Test]
    public async Task Logout()
    {
        var authenticationService = Container.Resolve<IAuthenticationService>();

        LogoutResponse logoutResponse = await authenticationService.RequestLogoutAsync(Token);

        Assert.Multiple(() =>
        {
            Assert.That(logoutResponse.Success, Is.True);
        });

        Console.WriteLine(logoutResponse.Message);
    }
}
