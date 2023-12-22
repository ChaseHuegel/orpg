using DryIoc;
using Moq;
using Orpg.Client.Services;
using Orpg.Shared.Models;

namespace Orpg.Client.Tests;

public class WorldServiceTests : TestBase
{
    private static readonly Character Character = new(
        uid: 1,
        name: "Adventurer",
        level: 1,
        archetypeId: 0,
        raceId: 0,
        location: "{locTestland}",
        activity: "{actCamping}"
    );

    private static readonly CharacterMetadata CharacterMetadata = new(
        uid: Character.Uid,
        xp: 0,
        equipment: new Inventory(
            size: (int)EquipmentSlot.Count,
            new ItemSlot(slot: (int)EquipmentSlot.MainHand, new ItemStack(uid: 1, count: 1))
        ),
        attributes: Array.Empty<AttributeData>(),
        skills: Array.Empty<SkillData>()
    );

    private static readonly PlayerSnapshot PlayerSnapshot = new(
        character: Character,
        metadata: CharacterMetadata,
        position: new Position(),
        inventory: new Inventory(),
        abilities: Array.Empty<AbilityData>()
    );

    protected override void Setup(Container container)
    {
        var mockWorldService = new Mock<IWorldService>();

        mockWorldService.Setup(
            worldService => worldService.RequestEntryAsync(Character.Uid)
        ).ReturnsAsync(new WorldEntryResponse(true, $"Entered world as \"{Character.Name}\".", PlayerSnapshot));

        mockWorldService.Setup(
            worldService => worldService.RequestLeaveAsync(Character.Uid)
        ).ReturnsAsync(new WorldLeaveResponse(true, $"Left world as \"{Character.Name}\"."));

        container.RegisterInstance(mockWorldService.Object);
    }

    [Test]
    public async Task Enter()
    {
        var worldService = Container.Resolve<IWorldService>();

        WorldEntryResponse worldEntryResponse = await worldService.RequestEntryAsync(Character.Uid);

        Assert.Multiple(() =>
        {
            Assert.That(worldEntryResponse.Success, Is.True);
        });

        Console.WriteLine(worldEntryResponse.Message);
    }

    [Test]
    public async Task Leave()
    {
        var worldService = Container.Resolve<IWorldService>();

        WorldLeaveResponse worldEntryResponse = await worldService.RequestLeaveAsync(Character.Uid);

        Assert.Multiple(() =>
        {
            Assert.That(worldEntryResponse.Success, Is.True);
        });

        Console.WriteLine(worldEntryResponse.Message);
    }
}
