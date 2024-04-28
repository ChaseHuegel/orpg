using System.Net;
using System.Net.Sockets;
using DryIoc;
using Orpg.Networking.Events;
using Orpg.Networking.LowLevel;
using Orpg.Networking.Messaging;
using Orpg.Shared.Data;
using Orpg.Shared.Serialization;

namespace Orpg.Client.Tests;

internal class TcpTests: TestBase
{
    internal class LengthDelimitedTcpService : IDataService
    {
        public event EventHandler<DataEventArgs>? Received;

        public void Start(IPEndPoint endPoint)
        {
            Task.Run(() => RunTcpServer(endPoint));
            Task.Run(() => RunTcpClient(endPoint));
        }

        private async Task RunTcpServer(IPEndPoint endPoint)
        {
            TcpListener tcpServer = new(endPoint.Address, endPoint.Port);
            tcpServer.Start();

            TcpClient client = await tcpServer.AcceptTcpClientAsync();
            NetworkStream stream = client.GetStream();

            Packet[] packets = new Packet[] {
                new(0, new TextMessage("The quick").Serialize()),
                new(0, new TextMessage("brown fox jumped").Serialize()),
                new(0, new TextMessage("over the fence.").Serialize()),
            };

            foreach (Packet packet in packets)
            {
                byte[] packetBuffer = packet.Serialize();
                byte[] buffer = new byte[4 + packetBuffer.Length];
                byte[] lengthDelimiter = BitConverter.GetBytes(buffer.Length);
                lengthDelimiter.CopyTo(buffer, 0);
                packetBuffer.CopyTo(buffer, lengthDelimiter.Length);

                await stream.WriteAsync(buffer);
            }

            stream.Close();
        }

        private async Task RunTcpClient(IPEndPoint endPoint)
        {
            TcpClient tcpClient = new();
            await tcpClient.ConnectAsync(endPoint);

            NetworkStream stream = tcpClient.GetStream();

            int lengthToRead = -1;
            int dataBufferOffset = 0;
            byte[] dataBuffer = new byte[2048];
            byte[] readBuffer = new byte[256];
            try
            {
                while (tcpClient.Connected)
                {
                    int bytesRead = await stream.ReadAsync(readBuffer);
                    if (bytesRead == 0)
                    {
                        stream.Close();
                    }

                    Array.Copy(readBuffer, 0, dataBuffer, dataBufferOffset, bytesRead);
                    dataBufferOffset += bytesRead;

                    if (lengthToRead == -1 && dataBufferOffset >= 4)
                    {
                        lengthToRead = BitConverter.ToInt32(dataBuffer, 0);
                    }

                    if (dataBufferOffset >= lengthToRead)
                    {
                        byte[] data = dataBuffer[..lengthToRead];

                        Array.Copy(dataBuffer, lengthToRead, dataBuffer, 0, dataBuffer.Length - lengthToRead);
                        dataBufferOffset -= lengthToRead;
                        lengthToRead = -1;

                        Received?.Invoke(this, new DataEventArgs(data));
                    }
                }
            }
            catch (Exception ex)
            {
                tcpClient.Dispose();
            }
        }
    }

    internal class NoOpParser : IParser
    {
        public List<byte[]> Parse(byte[] data)
        {
            return new List<byte[]>() { data };
        }
    }

    internal class TextMessageSerializer : IPacketSerializer<TextMessage>
    {
        public ushort PacketId => 0;

        public byte[] Serialize(TextMessage value)
        {
            return value.Serialize();
        }

        public TextMessage Deserialize(byte[] data)
        {
            return TextMessage.Deserialize(data, 0, data.Length);
        }
    }

    protected override void Setup(Container container)
    {
        container.Register<IDataService, LengthDelimitedTcpService>(Reuse.Singleton, serviceKey: "text");
        container.RegisterMapping<LengthDelimitedTcpService, IDataService>();
        container.Register<IParser, NoOpParser>(serviceKey: "text");
        container.Register<IDataProducer, DataProducer>(serviceKey: "text", setup: DryIoc.Setup.With(trackDisposableTransient: true), made: Parameters.Of.Type<IParser>(serviceKey: "text").Type<IDataService[]>(serviceKey: "text"));
        container.Register<IPacketSerializer<TextMessage>, TextMessageSerializer>(serviceKey: "text");
        container.Register<IMessageProducer<TextMessage>, PacketProducer<TextMessage>>(Reuse.Singleton, serviceKey: "text", setup: DryIoc.Setup.With(trackDisposableTransient: true), made: Parameters.Of.Type<IPacketSerializer<TextMessage>>(serviceKey: "text"));
        container.Register<IMessageConsumer<TextMessage>, MessageConsumer<TextMessage>>(Reuse.Singleton, serviceKey: "text", setup: DryIoc.Setup.With(trackDisposableTransient: true));
    }

    [Test]
    public async Task ReceiveTextMessage()
    {
        var tcpService = Container.Resolve<LengthDelimitedTcpService>();
        var messageConsumer = Container.Resolve<IMessageConsumer<TextMessage>>(serviceKey: "text");

        var tcs = new TaskCompletionSource<TextMessage>();

        int messageCount = 0;
        messageConsumer.NewMessage += onNewMessage;

        void onNewMessage(object? sender, TextMessage e)
        {
            Console.WriteLine("Message: " + e);

            messageCount++;
            if (messageCount == 3)
            {
                tcs.SetResult(e);
            }
        }

        var endPoint = new IPEndPoint(IPAddress.Loopback, 1234);
        tcpService.Start(endPoint);

        TextMessage response = await tcs.Task;
        messageConsumer.NewMessage -= onNewMessage;
        Assert.That(response.Message, Is.EqualTo("over the fence."));
    }
}