using DryIoc;
using Moq;
using Orpg.Shared.Models;
using System.Net;

namespace Orpg.Client.Tests;

public class WorldServiceTests : TestBase
{
    private const string Token = "1234";

    private static readonly WorldServer WorldServer = new(
        uid: 0,
        name: "Test",
        status: "{statusEmpty}",
        endPoint: new IPEndPoint(IPAddress.Loopback, 1234)
    );

    private static readonly Character Character = new(
        uid: 0,
        name: "Adventurer",
        level: 1,
        archetypeId: 0,
        raceId: 0,
        location: "{locTestland}",
        activity: "{actCamping}",
        appearance: new Appearance(),
        visuals: new Visuals()
    );

    private static readonly WorldServer[] WorldList = new WorldServer[] {
        WorldServer
    };

    protected override void Setup(Container container)
    {
        var mockWorldService = new Mock<IWorldService>();
        mockWorldService.Setup(
            worldService => worldService.RequestJoinAsync(WorldServer, Token, Character.Uid)
        ).ReturnsAsync(new WorldJoinResponse(true, "Joined world."));

        mockWorldService.Setup(
            worldService => worldService.RequestWorldListAsync()
        ).ReturnsAsync(new WorldListResponse(true, "Retrieved world list.", WorldList));

        container.RegisterInstance(mockWorldService.Object);
    }

    [Test]
    public async Task GetWorldList()
    {
        var worldService = Container.Resolve<IWorldService>();

        WorldListResponse worldListResponse = await worldService.RequestWorldListAsync();

        Assert.Multiple(() =>
        {
            Assert.That(worldListResponse.Success, Is.True);
            Assert.That(worldListResponse.Worlds, Is.Not.Empty);
            Assert.That(worldListResponse.Worlds[0].Uid, Is.EqualTo(WorldList[0].Uid));
        });

        Console.WriteLine(worldListResponse.Message);
    }

    [Test]
    public async Task JoinWorld()
    {
        var worldService = Container.Resolve<IWorldService>();

        WorldJoinResponse worldJoinResponse = await worldService.RequestJoinAsync(WorldServer, Token, Character.Uid);

        Assert.Multiple(() =>
        {
            Assert.That(worldJoinResponse.Success, Is.True);
        });

        Console.WriteLine(worldJoinResponse.Message);
    }
}
