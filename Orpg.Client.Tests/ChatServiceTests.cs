using DryIoc;
using Moq;
using Orpg.Client.Services;
using Orpg.Shared.Models;
using Orpg.Shared.Responses;

namespace Orpg.Client.Tests;

public class ChatServiceTests : TestBase
{
    private static readonly int ParticipantUid = 1;

    private static readonly ChatChannel[] Channels = new ChatChannel[] {
        new(
            uid: 0,
            name: "{chatGeneral}"
        )
    };

    private static readonly Chat ChatToSend = new(
        channelUid: Channels[0].Uid,
        senderUid: 1,
        message: "A test chat message."
    );

    protected override void Setup(Container container)
    {
        var mockChatService = new Mock<IChatService>();

        mockChatService.Setup(
            chatService => chatService.RequestJoinAsync(ParticipantUid)
        ).ReturnsAsync(new ChatJoinResponse(true, $"Joined chat as \"{ParticipantUid}\", with ({Channels.Length}) available channels.", Channels));

        mockChatService.Setup(
            chatService => chatService.RequestLeaveAsync(ParticipantUid)
        ).ReturnsAsync(new ChatLeaveResponse(true, $"Left chat as \"{ParticipantUid}\"."));

        mockChatService.SetupAdd(chatService => chatService.Received += It.IsAny<EventHandler<ChatEventArgs>>());
        mockChatService.SetupRemove(chatService => chatService.Received -= It.IsAny<EventHandler<ChatEventArgs>>());
        mockChatService.Setup(
            chatService => chatService.StartListening()
        ).Raises(chatService => chatService.Received += null, this, new ChatEventArgs(ChatToSend));

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

    [Test]
    public async Task ReceiveChat()
    {
        var chatService = Container.Resolve<IChatService>();

        var tcs = new TaskCompletionSource<Chat>();

        chatService.Received += onChatReceived;
        chatService.StartListening();

        void onChatReceived(object? sender, ChatEventArgs e)
        {
            tcs.SetResult(e.Chat);
        }

        Chat chat = await tcs.Task;
        chatService.Received -= onChatReceived;
        chatService.StopListening();

        Assert.Multiple(() =>
        {
            Assert.That(chat, Is.EqualTo(ChatToSend));
        });

        Console.WriteLine(chat.ToString());
    }
}