using DryIoc;
using Moq;
using Orpg.Client.Services;
using Orpg.Shared.Models;
using Orpg.Shared.Responses;
using System.Net;

namespace Orpg.Client.Tests;

public class ClusterServiceTests : TestBase
{
    private const string SessionToken = "5678";

    private static readonly ClusterServer Cluster = new(
        uid: 1,
        name: "Test",
        status: "{statusEmpty}",
        endPoint: new IPEndPoint(IPAddress.Loopback, 1234)
    );

    private static readonly ClusterServer[] ClusterList = new ClusterServer[] {
        Cluster
    };

    protected override void Setup(Container container)
    {
        var mockClusterService = new Mock<IClusterService>();

        mockClusterService.Setup(
            clusterService => clusterService.RequestJoinAsync(Cluster, SessionToken)
        ).ReturnsAsync(new ClusterJoinResponse(true, $"Joined cluster \"{Cluster.Name}\"."));

        mockClusterService.Setup(
            clusterService => clusterService.RequestServerListAsync()
        ).ReturnsAsync(new ClusterListResponse(true, "Retrieved server list.", ClusterList));

        container.RegisterInstance(mockClusterService.Object);
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

        ClusterJoinResponse clusterJoinResponse = await clusterService.RequestJoinAsync(Cluster, SessionToken);

        Assert.Multiple(() =>
        {
            Assert.That(clusterJoinResponse.Success, Is.True);
        });

        Console.WriteLine(clusterJoinResponse.Message);
    }
}
