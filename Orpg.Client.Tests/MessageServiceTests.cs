using DryIoc;
using Orpg.Networking.Events;
using Orpg.Networking.LowLevel;
using Orpg.Networking.Messaging;
using Orpg.Shared.Serialization;
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
        container.Register<IDataService, SimpleDataService>(Reuse.Singleton, serviceKey: "text");
        container.RegisterMapping<SimpleDataService, IDataService>();
        container.Register<IParser, CsvParser>(serviceKey: "text");
        container.Register<IDataProducer, DataProducer>(serviceKey: "text", setup: DryIoc.Setup.With(trackDisposableTransient: true), made: Parameters.Of.Type<IParser>(serviceKey: "text").Type<IDataService[]>(serviceKey: "text"));
        container.Register<ISerializer<string>, ASCIISerializer>(serviceKey: "text");
        container.Register<IMessageProducer<string>, MessageProducer<string>>(Reuse.Singleton, serviceKey: "text", setup: DryIoc.Setup.With(trackDisposableTransient: true), made: Parameters.Of.Type<ISerializer<string>>(serviceKey: "text"));
        container.Register<IMessageConsumer<string>, MessageConsumer<string>>(Reuse.Singleton, serviceKey: "text", setup: DryIoc.Setup.With(trackDisposableTransient: true));
    }

    [Test]
    public async Task Receive()
    {
        var simpleDataService = Container.Resolve<SimpleDataService>();
        var messageConsumer = Container.Resolve<IMessageConsumer<string>>(serviceKey: "text");

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