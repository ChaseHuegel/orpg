using DryIoc;

namespace Orpg.Client.Tests;

public abstract class TestBase
{
    protected Container Container { get; private set; }

    [SetUp]
    public void SetupInternal()
    {
        Container = new Container();
        Setup(Container);
        Container.ValidateAndThrow();
    }

    protected abstract void Setup(Container container);
}