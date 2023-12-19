using DryIoc;
using Moq;
using Orpg.Client.Services;
using Orpg.Shared.Models;

namespace Orpg.Client.Tests;

public class AccountCharacterTests : TestBase
{
    private const string Token = "1234";
    private static readonly CharacterSnippet[] CharacterList = new CharacterSnippet[] {
        new(uid: 0, name: "Adventurer", description: "Level 1 Unit Tester", location: "Testland", activity: "Swimming with the fishes.")
    };

    protected override void Setup(Container container)
    {
        var mockAccountService = new Mock<IAccountService>();
        mockAccountService.Setup(
            accountService => accountService.RequestCharacterListAsync(Token)
        ).ReturnsAsync(new CharacterListResponse(true, "Retrieved character list.", CharacterList));

        container.RegisterInstance(mockAccountService.Object);
    }

    [Test]
    public async Task GetCharacterList()
    {
        var accountService = Container.Resolve<IAccountService>();

        CharacterListResponse characterListResponse = await accountService.RequestCharacterListAsync(Token);

        Assert.Multiple(() =>
        {
            Assert.That(characterListResponse.Success, Is.True);
            Assert.That(characterListResponse.Characters, Is.Not.Empty);
            Assert.That(characterListResponse.Characters[0].Name, Is.EqualTo(CharacterList[0].Name));
        });
    }
}
