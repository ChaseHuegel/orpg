using DryIoc;
using Moq;
using Orpg.Shared.Services;
using System.Text;

namespace Orpg.Client.Tests;

internal class MessageServiceTests: TestBase
{
    private const int MessageType = 1;

    private readonly Mock<IRawMessageHandler> MockMessageHandler = new();

    protected override void Setup(Container container)
    {
        MockMessageHandler.SetupGet(messageHandler => messageHandler.MessageType)
            .Returns(MessageType);

        var mockMessageService = new Mock<IRawMessageService>();

        mockMessageService.Setup(messageService => messageService.Post(MessageType, It.IsAny<byte[]>()))
            .Callback<int, byte[]>((type, data) => MockMessageHandler.Object.Handle(data));

        container.RegisterInstance(MockMessageHandler.Object);
        container.RegisterInstance(mockMessageService.Object);
    }

    [Test]
    public async Task ReceiveMessage()
    {
        var messageService = Container.Resolve<IRawMessageService>();
        var tcs = new TaskCompletionSource<byte[]>();

        MockMessageHandler.Setup(messageHandler => messageHandler.Handle(It.IsAny<byte[]>()))
            .Callback(HandleMessage);

        void HandleMessage(byte[] data)
        {
            tcs.SetResult(data);
        }

        var dataToPost = Encoding.UTF8.GetBytes("Test message.");
        messageService.Post(1, dataToPost);

        byte[] data = await tcs.Task;

        Assert.Multiple(() =>
        {
            Assert.That(data, Is.EqualTo(dataToPost));
        });

        Console.WriteLine(Encoding.UTF8.GetString(data));
    }
}