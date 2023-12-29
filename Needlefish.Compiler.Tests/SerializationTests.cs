using Lexer.Tests;
using NUnit.Framework;

namespace Needlefish.Compiler.Tests;

internal class SerializationTests
{
    [Test]
    public void SerializeDeserialize()
    {
        var message = new TestMessage();
        message.Int = 325;

        var data = message.Serialize();

        message.Deserialize(data);
        Assert.Multiple(() =>
        {
            Assert.That(message.Int, Is.EqualTo(325));
            Assert.That(message.OptionalInt, Is.EqualTo(null));
        });
    }
}
