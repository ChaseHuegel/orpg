using Lexer.Tests;
using NUnit.Framework;
using System;

namespace Needlefish.Compiler.Tests;

internal class SerializationTests
{
    [Test]
    public void RoundtripInt()
    {
        var message = new TestMessage
        {
            Int = 325
        };

        var data = message.Serialize();

        message.Unpack(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(10));
            Assert.That(message.Int, Is.EqualTo(325));
        });
    }

    [Test]
    public void RoundtripOptionalInt()
    {
        var message = new TestMessage
        {
            OptionalInt = 68
        };

        var data = message.Serialize();

        message.Unpack(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(17));
            Assert.That(message.OptionalInt, Is.EqualTo(68));
        });
    }

    [Test]
    public void RoundTripInts()
    {
        var expectedInts = new int[] { 1, 2, 3, 4 };

        var message = new TestMessage
        {
            Ints = expectedInts
        };

        var data = message.Serialize();

        message.Unpack(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(26));
            Assert.That(message.Ints, Is.EqualTo(expectedInts));
        });
    }

    [Test]
    public void RoundTripOptionalInts()
    {
        var expectedOptionalInts = new int[] { 5, 6, 7, 8 };

        var message = new TestMessage
        {
            OptionalInts = expectedOptionalInts
        };

        var data = message.Serialize();

        message.Unpack(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(31));
            Assert.That(message.OptionalInts, Is.EqualTo(expectedOptionalInts));
        });
    }

    [Test]
    public void RoundTripAll()
    {
        var expectedInts = new int[] { 1, 2, 3, 4 };
        var expectedOptionalInts = new int[] { 5, 6, 7, 8 };

        var message = new TestMessage
        {
            Int = 325,
            OptionalInt = 68,
            Ints = expectedInts,
            OptionalInts = expectedOptionalInts
        };

        var data = message.Serialize();

        message.Unpack(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(54));
            Assert.That(message.Int, Is.EqualTo(325));
            Assert.That(message.OptionalInt, Is.EqualTo(68));
            Assert.That(message.Ints, Is.EqualTo(expectedInts));
            Assert.That(message.OptionalInts, Is.EqualTo(expectedOptionalInts));
        });
    }

    [Test]
    public void RoundTripAllWithOptionals()
    {
        var expectedInts = new int[] { 1, 2, 3, 4 };

        var message = new TestMessage
        {
            Int = 325,
            OptionalInt = null,
            Ints = expectedInts,
            OptionalInts = null
        };

        var data = message.Serialize();

        message.Unpack(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(26));
            Assert.That(message.Int, Is.EqualTo(325));
            Assert.That(message.OptionalInt, Is.EqualTo(null));
            Assert.That(message.Ints, Is.EqualTo(expectedInts));
            Assert.That(message.OptionalInts, Is.EqualTo(null));
        });
    }
}
