using DryIoc;
using Moq;
using Orpg.Client.Services;
using Orpg.Shared.Models;

namespace Orpg.Client.Tests;

public class AccountManagementTests : TestBase
{
    private const string Username = "default";
    private const string Password = "password";

    protected override void Setup(Container container)
    {
        var mockAccountService = new Mock<IAccountService>();
        mockAccountService.Setup(
            accountService => accountService.RequestRegistrationAsync(new BasicAuthentication(Username, Password))
        ).ReturnsAsync(new RegistrationResponse(true, $"Registered \"{Username}\"."));

        container.RegisterInstance(mockAccountService.Object);
    }

    [Test]
    public async Task Register()
    {
        var accountService = Container.Resolve<IAccountService>();

        RegistrationResponse registrationResponse = await accountService.RequestRegistrationAsync(new BasicAuthentication(Username, Password));

        Assert.That(registrationResponse.Success, Is.True);

        Console.WriteLine(registrationResponse.Message);
    }
}
