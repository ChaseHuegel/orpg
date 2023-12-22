using DryIoc;
using Moq;
using Orpg.Shared.Models;
using System.Net;

namespace Orpg.Client.Tests;

public class ClusterServiceTests : TestBase
{
    private const string Token = "1234";

    private static readonly ClusterServer Cluster = new(
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

    private static readonly ClusterServer[] ClusterList = new ClusterServer[] {
        Cluster
    };

    protected override void Setup(Container container)
    {
        var mockWorldService = new Mock<IClusterService>();
        mockWorldService.Setup(
            clusterService => clusterService.RequestJoinAsync(Cluster, Token, Character.Uid)
        ).ReturnsAsync(new ClusterJoinResponse(true, $"Joined cluster \"{Cluster.Name}\"."));

        mockWorldService.Setup(
            clusterService => clusterService.RequestServerListAsync()
        ).ReturnsAsync(new ClusterListResponse(true, "Retrieved world list.", ClusterList));

        container.RegisterInstance(mockWorldService.Object);
    }

    [Test]
    public async Task GetServerList()
    {
        var clusterService = Container.Resolve<IClusterService>();

        ClusterListResponse clusterListResponse = await clusterService.RequestServerListAsync();

        Assert.Multiple(() =>
        {
            Assert.That(clusterListResponse.Success, Is.True);
            Assert.That(clusterListResponse.Servers, Is.Not.Empty);
            Assert.That(clusterListResponse.Servers[0].Uid, Is.EqualTo(ClusterList[0].Uid));
        });

        Console.WriteLine(clusterListResponse.Message);
    }

    [Test]
    public async Task Join()
    {
        var clusterService = Container.Resolve<IClusterService>();

        ClusterJoinResponse clusterJoinResponse = await clusterService.RequestJoinAsync(Cluster, Token, Character.Uid);

        Assert.Multiple(() =>
        {
            Assert.That(clusterJoinResponse.Success, Is.True);
        });

        Console.WriteLine(clusterJoinResponse.Message);
    }
}
