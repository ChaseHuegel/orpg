using DryIoc;
using Moq;
using Orpg.Client.Services;
using Orpg.Shared.Models;

namespace Orpg.Client.Tests;

public class AccountDataTests : TestBase
{
    private const string Token = "1234";
    private static readonly Character[] CharacterList = new Character[] {
        new(
            uid: 1,
            name: "Adventurer",
            level: 1,
            archetypeId: 0,
            raceId: 0,
            location: "{locTestland}",
            activity: "{actCamping}"
        )
    };

    protected override void Setup(Container container)
    {
        var mockAccountService = new Mock<IAccountService>();

        mockAccountService.Setup(
            accountService => accountService.RequestCharacterListAsync(Token)
        ).ReturnsAsync(new CharacterListResponse(true, "Retrieved character list.", CharacterList));

        mockAccountService.Setup(
            accountService => accountService.RequestCharacterDeletion(Token, CharacterList[0].Uid)
        ).ReturnsAsync(new CharacterDeletionResponse(true, $"Deleted character \"{CharacterList[0].Name}\"."));

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
            Assert.That(characterListResponse.Characters[0].Uid, Is.EqualTo(CharacterList[0].Uid));
        });

        Console.WriteLine(characterListResponse.Message);
    }

    [Test]
    public async Task DeleteCharacter()
    {
        var accountService = Container.Resolve<IAccountService>();

        CharacterDeletionResponse characterDeletionResponse = await accountService.RequestCharacterDeletion(Token, CharacterList[0].Uid);

        Assert.Multiple(() =>
        {
            Assert.That(characterDeletionResponse.Success, Is.True);
        });

        Console.WriteLine(characterDeletionResponse.Message);
    }
}
