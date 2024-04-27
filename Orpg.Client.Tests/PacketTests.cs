using DryIoc;
using Orpg.Networking.LowLevel;
using Orpg.Networking.Messaging;
using Orpg.Networking.Services;
using Orpg.Shared.Data;
using Orpg.Shared.Serialization;

namespace Orpg.Client.Tests;

internal partial class PacketTests: TestBase
{
    internal class CharacterCreationResponseSerializer : IPacketSerializer<CharacterCreationResponse>
    {
        public ushort PacketId => 0;

        public byte[] Serialize(CharacterCreationResponse value)
        {
            return value.Serialize();
        }

        public CharacterCreationResponse Deserialize(byte[] data)
        {
            return CharacterCreationResponse.Deserialize(data, 0, data.Length);
        }
    }

    internal class CharacterDeletionResponseSerializer : IPacketSerializer<CharacterDeletionResponse>
    {
        public ushort PacketId => 1;

        public byte[] Serialize(CharacterDeletionResponse value)
        {
            return value.Serialize();
        }

        public CharacterDeletionResponse Deserialize(byte[] data)
        {
            return CharacterDeletionResponse.Deserialize(data, 0, data.Length);
        }
    }

    protected override void Setup(Container container)
    {
        container.Register<IDataService, SimpleDataService>(Reuse.Singleton, serviceKey: "text");
        container.RegisterMapping<SimpleDataService, IDataService>();
        container.Register<IParser, LengthDelimitedParser>(serviceKey: "text");
        container.Register<IDataProducer, DataProducer>(serviceKey: "text", setup: DryIoc.Setup.With(trackDisposableTransient: true), made: Parameters.Of.Type<IParser>(serviceKey: "text").Type<IDataService[]>(serviceKey: "text"));
        container.Register<IPacketSerializer<CharacterCreationResponse>, CharacterCreationResponseSerializer>(serviceKey: "text");
        container.Register<IPacketSerializer<CharacterDeletionResponse>, CharacterDeletionResponseSerializer>(serviceKey: "text");
        container.Register<IMessageProducer<CharacterCreationResponse>, PacketProducer<CharacterCreationResponse>>(Reuse.Singleton, serviceKey: "text", setup: DryIoc.Setup.With(trackDisposableTransient: true), made: Parameters.Of.Type<IPacketSerializer<CharacterCreationResponse>>(serviceKey: "text"));
        container.Register<IMessageConsumer<CharacterCreationResponse>, MessageConsumer<CharacterCreationResponse>>(Reuse.Singleton, serviceKey: "text", setup: DryIoc.Setup.With(trackDisposableTransient: true));
        container.Register<IMessageProducer<CharacterDeletionResponse>, PacketProducer<CharacterDeletionResponse>>(Reuse.Singleton, serviceKey: "text", setup: DryIoc.Setup.With(trackDisposableTransient: true), made: Parameters.Of.Type<IPacketSerializer<CharacterDeletionResponse>>(serviceKey: "text"));
        container.Register<IMessageConsumer<CharacterDeletionResponse>, MessageConsumer<CharacterDeletionResponse>>(Reuse.Singleton, serviceKey: "text", setup: DryIoc.Setup.With(trackDisposableTransient: true));
    }

    private static CharacterCreationResponse CharacterCreationResponseSource = new CharacterCreationResponse(true, "Character created successfully.", new Character());
    private static CharacterDeletionResponse CharacterDeletionResponseSource = new CharacterDeletionResponse(true, "Character deleted successfully.");

    private byte[] CreateCombinedPacket()
    {
        byte[] packet1 = CharacterDeletionResponseSource.Serialize();
        byte[] packet2 = CharacterCreationResponseSource.Serialize();

        byte[] packet1Length = BitConverter.GetBytes(packet1.Length + 2);
        byte[] packet2Length = BitConverter.GetBytes(packet2.Length + 2);

        byte[] packet1Id = BitConverter.GetBytes((ushort)1);
        byte[] packet2Id = BitConverter.GetBytes((ushort)0);

        byte[] joinedPacket = new byte[12 + packet1.Length + packet2.Length];
        packet1Length.CopyTo(joinedPacket, 0);
        packet1Id.CopyTo(joinedPacket, 4);
        packet1.CopyTo(joinedPacket, 6);

        packet2Length.CopyTo(joinedPacket, packet1.Length + 6 + 0);
        packet2Id.CopyTo(joinedPacket, packet1.Length + 6 + 4);
        packet2.CopyTo(joinedPacket, packet1.Length + 6 + 6);

        return joinedPacket;
    }

    [Test]
    public async Task ReceiveCharacterCreation()
    {
        var simpleDataService = Container.Resolve<SimpleDataService>();
        var messageConsumer = Container.Resolve<IMessageConsumer<CharacterCreationResponse>>(serviceKey: "text");

        var tcs = new TaskCompletionSource<CharacterCreationResponse>();

        messageConsumer.NewMessage += onNewMessage;

        void onNewMessage(object? sender, CharacterCreationResponse e)
        {
            tcs.SetResult(e);
        }

        simpleDataService.Post(CreateCombinedPacket());

        CharacterCreationResponse characterCreationResponse = await tcs.Task;
        messageConsumer.NewMessage -= onNewMessage;

        Assert.That(characterCreationResponse.Message, Is.EqualTo(CharacterCreationResponseSource.Message));
    }
}