namespace Orpg.Shared.Services;

public class ASCIIMessageProducer : MessageProducer<string>
{
    public override string Format => "ASCII";

    public ASCIIMessageProducer(ISerializer<string> serializer, IDataProducer[] dataProducers) : base(serializer, dataProducers)
    {
    }
}