using DryIoc;
using Moq;
using Orpg.Client.Services;
using Orpg.Shared.Models;

namespace Orpg.Client.Tests;

public class ChatServiceTests : TestBase
{
    private static readonly int ParticipantUid = 1;

    private readonly ChatChannel[] Channels = new ChatChannel[] {
        new(
            uid: 0,
            name: "{chatGeneral}"
        )
    };

    protected override void Setup(Container container)
    {
        var mockChatService = new Mock<IChatService>();

        mockChatService.Setup(
            chatService => chatService.RequestJoinAsync(ParticipantUid)
        ).ReturnsAsync(new ChatJoinResponse(true, $"Joined chat as \"{ParticipantUid}\", with ({Channels.Length}) available channels.", Channels));

        mockChatService.Setup(
            chatService => chatService.RequestLeaveAsync(ParticipantUid)
        ).ReturnsAsync(new ChatLeaveResponse(true, $"Left chat as \"{ParticipantUid}\"."));

        container.RegisterInstance(mockChatService.Object);
    }

    [Test]
    public async Task Join()
    {
        var chatService = Container.Resolve<IChatService>();

        ChatJoinResponse chatJoinResponse = await chatService.RequestJoinAsync(ParticipantUid);

        Assert.Multiple(() =>
        {
            Assert.That(chatJoinResponse.Success, Is.True);
            Assert.That(chatJoinResponse.Channels[0], Is.EqualTo(Channels[0]));
        });

        Console.WriteLine(chatJoinResponse.Message);
    }

    [Test]
    public async Task Leave()
    {
        var chatService = Container.Resolve<IChatService>();

        ChatLeaveResponse chatLeaveResponse = await chatService.RequestLeaveAsync(ParticipantUid);

        Assert.Multiple(() =>
        {
            Assert.That(chatLeaveResponse.Success, Is.True);
        });

        Console.WriteLine(chatLeaveResponse.Message);
    }
}
