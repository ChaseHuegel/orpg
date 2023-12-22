using DryIoc;
using Moq;
using Orpg.Client.Services;
using Orpg.Shared.Models;
using Orpg.Shared.Responses;

namespace Orpg.Client.Tests;

public class AccountManagementTests : TestBase
{
    private static readonly BasicAuthentication Credentials = new("default", "password");
    private const string Email = "test@test.com";

    protected override void Setup(Container container)
    {
        var mockAccountService = new Mock<IAccountService>();

        mockAccountService.Setup(
            accountService => accountService.RequestRegistrationAsync(Credentials, Email)
        ).ReturnsAsync(new RegistrationResponse(true, $"Registered \"{Credentials.Username}\"."));

        container.RegisterInstance(mockAccountService.Object);
    }

    [Test]
    public async Task Register()
    {
        var accountService = Container.Resolve<IAccountService>();

        RegistrationResponse registrationResponse = await accountService.RequestRegistrationAsync(Credentials, Email);

        Assert.That(registrationResponse.Success, Is.True);

        Console.WriteLine(registrationResponse.Message);
    }
}
