namespace Orpg.Shared.Services;

public class ASCIIDataProducer : DataProducer
{
    public override string Format => "ASCII";

    public ASCIIDataProducer(IDataService dataService, IParser parser) : base(dataService, parser)
    {
    }
}