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
        ).ReturnsAsync(new TokenResponse(true, $"Authenticated as \"{Username}\" successfully.", Token));

        mockAuthenticationService.Setup(
            authenticationService => authenticationService.RequestSessionAsync(Token)
        ).ReturnsAsync(new SessionResponse(true, $"Logged in as \"{Username}\" successfully.", SessionUid));

        container.RegisterInstance(mockAuthenticationService.Object);
    }

    [Test]
    public async Task GetAuthenticationToken()
    {
        var authenticationService = Container.Resolve<IAuthenticationService>();

        var credentials = new BasicAuthentication(Username, Password);
        TokenResponse tokenResponse = await authenticationService.RequestTokenAsync(credentials);

        Assert.Multiple(() =>
        {
            Assert.That(tokenResponse.Success, Is.True);
            Assert.That(tokenResponse.Token, Is.Not.Null);
        });
    }

    [Test]
    public async Task GetSession()
    {
        var authenticationService = Container.Resolve<IAuthenticationService>();

        var credentials = new BasicAuthentication(Username, Password);
        TokenResponse tokenResponse = await authenticationService.RequestTokenAsync(credentials);
        SessionResponse sessionResponse = await authenticationService.RequestSessionAsync(tokenResponse.Token);

        Assert.Multiple(() =>
        {
            Assert.That(sessionResponse.Success, Is.True);
            Assert.That(sessionResponse.Uid, Is.Not.EqualTo(0));
        });
    }
}
