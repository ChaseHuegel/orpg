using Lexer.Tests;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    [TestCase(1000)]
    [TestCase(10000)]
    [TestCase(100000)]
    public void BenchmarkSerialize(int iterations)
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

        long memBefore = GC.GetAllocatedBytesForCurrentThread();
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            byte[] data = message.Serialize();
        }
        sw.Stop();
        long memAfter = GC.GetAllocatedBytesForCurrentThread();
        byte[] sizeTest = message.Serialize();
        Assert.Pass(sw.Elapsed.ToString() + " " + (memAfter - memBefore) + $" bytes (serialized size: {sizeTest.Length})");
    }

    [Test]
    [TestCase(1000)]
    [TestCase(10000)]
    [TestCase(100000)]
    public void BenchmarkSerializeJson(int iterations)
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

        long memBefore = GC.GetAllocatedBytesForCurrentThread(); 
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            byte[] data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message));
        }
        sw.Stop();
        long memAfter = GC.GetAllocatedBytesForCurrentThread();
        byte[] sizeTest = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message));
        Assert.Pass(sw.Elapsed.ToString() + " " + (memAfter - memBefore) + $" bytes (serialized size: {sizeTest.Length})");
    }

    [Test]
    [TestCase(1000)]
    [TestCase(10000)]
    [TestCase(100000)]
    public void BenchmarkDeserialize(int iterations)
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

        byte[] data = message.Serialize();

        long memBefore = GC.GetAllocatedBytesForCurrentThread(); 
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            message.Unpack(data);
        }
        sw.Stop();
        long memAfter = GC.GetAllocatedBytesForCurrentThread();
        Assert.Pass(sw.Elapsed.ToString() + " " + (memAfter - memBefore) + " bytes");
    }

    [Test]
    [TestCase(1000)]
    [TestCase(10000)]
    [TestCase(100000)]
    public void BenchmarkDeserializeJson(int iterations)
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

        byte[] data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message));

        long memBefore = GC.GetAllocatedBytesForCurrentThread(); 
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            JsonConvert.DeserializeObject<TestMessage>(Encoding.ASCII.GetString(data));
        }
        sw.Stop();
        long memAfter = GC.GetAllocatedBytesForCurrentThread();
        Assert.Pass(sw.Elapsed.ToString() + " " + (memAfter - memBefore) + " bytes");
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
