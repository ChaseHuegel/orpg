using DryIoc;
using Moq;
using Orpg.Client.Services;
using Orpg.Shared.Models;
using Orpg.Shared.Responses;

namespace Orpg.Client.Tests;

public class AuthenticationTests : TestBase
{
    private const string Username = "default";
    private const string Password = "password";
    private const string AuthenticationToken = "1234";
    private const string SessionToken = "5678";

    protected override void Setup(Container container)
    {
        var mockAuthenticationService = new Mock<IAuthenticationService>();

        mockAuthenticationService.Setup(
            authenticationService => authenticationService.AuthenticateAsync(new BasicAuthentication(Username, Password))
        ).ReturnsAsync(new AuthenticationResponse(true, $"Authenticated as \"{Username}\".", AuthenticationToken));

        mockAuthenticationService.Setup(
            authenticationService => authenticationService.LoginAsync(AuthenticationToken)
        ).ReturnsAsync(new LoginResponse(true, $"Logged in as \"{Username}\".", SessionToken));

        mockAuthenticationService.Setup(
            authenticationService => authenticationService.LogoutAsync(SessionToken)
        ).ReturnsAsync(new LogoutResponse(true, $"Logged out as \"{Username}\"."));

        container.RegisterInstance(mockAuthenticationService.Object);
    }

    [Test]
    public async Task Authenticate()
    {
        var authenticationService = Container.Resolve<IAuthenticationService>();

        var credentials = new BasicAuthentication(Username, Password);
        AuthenticationResponse tokenResponse = await authenticationService.AuthenticateAsync(credentials);

        Assert.Multiple(() =>
        {
            Assert.That(tokenResponse.Success, Is.True);
            Assert.That(tokenResponse.AuthenticationToken, Is.Not.Null);
            Assert.That(tokenResponse.AuthenticationToken, Is.EqualTo(AuthenticationToken));
        });

        Console.WriteLine(tokenResponse.Message);
    }

    [Test]
    public async Task Login()
    {
        var authenticationService = Container.Resolve<IAuthenticationService>();

        var credentials = new BasicAuthentication(Username, Password);
        AuthenticationResponse authResponse = await authenticationService.AuthenticateAsync(credentials);
        LoginResponse loginResponse = await authenticationService.LoginAsync(authResponse.AuthenticationToken);

        Assert.Multiple(() =>
        {
            Assert.That(loginResponse.Success, Is.True);
            Assert.That(loginResponse.SessionToken, Is.Not.Null);
            Assert.That(loginResponse.SessionToken, Is.EqualTo(SessionToken));
        });

        Console.WriteLine(loginResponse.Message);
    }

    [Test]
    public async Task Logout()
    {
        var authenticationService = Container.Resolve<IAuthenticationService>();

        LogoutResponse logoutResponse = await authenticationService.LogoutAsync(SessionToken);

        Assert.Multiple(() =>
        {
            Assert.That(logoutResponse.Success, Is.True);
        });

        Console.WriteLine(logoutResponse.Message);
    }
}
