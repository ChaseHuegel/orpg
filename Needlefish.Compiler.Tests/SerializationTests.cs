﻿using Lexer.Tests;
using NUnit.Framework;

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

        message.Deserialize(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(16));
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

        message.Deserialize(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(20));
            Assert.That(message.OptionalInt, Is.EqualTo(68));
        });
    }

    [Test]
    public void RoundTripIntArray()
    {
        var expectedIntArray = new int[] { 1, 2, 3, 4 };

        var message = new TestMessage
        {
            IntArray = expectedIntArray
        };

        var data = message.Serialize();

        message.Deserialize(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(32));
            Assert.That(message.IntArray, Is.EqualTo(expectedIntArray));
        });
    }

    [Test]
    public void RoundTripOptionalIntArray()
    {
        var expectedOptionalIntArray = new int[] { 5, 6, 7, 8 };

        var message = new TestMessage
        {
            OptionalIntArray = expectedOptionalIntArray
        };

        var data = message.Serialize();

        message.Deserialize(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(34));
            Assert.That(message.OptionalIntArray, Is.EqualTo(expectedOptionalIntArray));
        });
    }

    [Test]
    public void RoundTripAll()
    {
        var expectedIntArray = new int[] { 1, 2, 3, 4 };
        var expectedOptionalIntArray = new int[] { 5, 6, 7, 8 };

        var message = new TestMessage
        {
            Int = 325,
            OptionalInt = 68,
            IntArray = expectedIntArray,
            OptionalIntArray = expectedOptionalIntArray
        };

        var data = message.Serialize();

        message.Deserialize(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(54));
            Assert.That(message.Int, Is.EqualTo(325));
            Assert.That(message.OptionalInt, Is.EqualTo(68));
            Assert.That(message.IntArray, Is.EqualTo(expectedIntArray));
            Assert.That(message.OptionalIntArray, Is.EqualTo(expectedOptionalIntArray));
        });
    }

    [Test]
    public void RoundTripAllWithOptionals()
    {
        var expectedIntArray = new int[] { 1, 2, 3, 4 };

        var message = new TestMessage
        {
            Int = 325,
            OptionalInt = null,
            IntArray = expectedIntArray,
            OptionalIntArray = null
        };

        var data = message.Serialize();

        message.Deserialize(data);
        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(32));
            Assert.That(message.Int, Is.EqualTo(325));
            Assert.That(message.OptionalInt, Is.EqualTo(null));
            Assert.That(message.IntArray, Is.EqualTo(expectedIntArray));
            Assert.That(message.OptionalIntArray, Is.EqualTo(null));
        });
    }
}
