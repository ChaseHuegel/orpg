using DryIoc;
using Orpg.Shared.Services;
using System.Text;

namespace Orpg.Client.Tests;

internal partial class MessageServiceTests: TestBase
{
    internal class SimpleDataService : IDataService
    {
        public event EventHandler<DataEventArgs>? Received;

        public void Post(byte[] bytes)
        {
            Received?.Invoke(this, new DataEventArgs(bytes));
        }
    }

    protected override void Setup(Container container)
    {
        var textMessagingContainer = container.CreateChild();
        container.Register<IDataService, SimpleDataService>(Reuse.Singleton);
        container.Register<IParser, CsvParser>();
        container.Register<IDataProducer, ASCIIDataProducer>(Reuse.Singleton, setup: DryIoc.Setup.With(trackDisposableTransient: true));
        container.Register<ISerializer<string>, ASCIISerializer>();
        container.Register<IMessageProducer<string>, ASCIIMessageProducer>(Reuse.Singleton, setup: DryIoc.Setup.With(trackDisposableTransient: true));
        container.Register<IMessageConsumer<string>, MessageConsumer<string>>(Reuse.Singleton, setup: DryIoc.Setup.With(trackDisposableTransient: true));
    }

    [Test]
    public async Task Receive()
    {
        var simpleDataService = (SimpleDataService)Container.Resolve<IDataService>();
        var messageConsumer = Container.Resolve<IMessageConsumer<string>>();

        var tcs = new TaskCompletionSource<string>();
        int messageCount = 0;

        messageConsumer.NewMessage += onNewMessage;

        void onNewMessage(object? sender, string e)
        {
            Console.WriteLine("Message: " + e);

            messageCount++;
            if (messageCount == 2)
            {
                tcs.SetResult(e);
            }
        }

        simpleDataService.Post(Encoding.ASCII.GetBytes("Hello World!,Hello again!"));

        string textMessage = await tcs.Task;
        messageConsumer.NewMessage -= onNewMessage;

        Assert.That(textMessage, Is.EqualTo("Hello again!"));
    }
}