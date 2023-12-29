using NUnit.Framework;

namespace Needlefish.Compiler.Tests;

internal class ByteConversionTests
{
    [TestCase((ushort)100)]
    public void UShort(ushort value)
    {
        byte[] buffer = new byte[2];

        buffer[0] = (byte)(value << 8);
        buffer[1] = (byte)value;

        ushort decodedValue = (ushort)((buffer[0] << 8) | buffer[1]);

        Assert.That(decodedValue, Is.EqualTo(value));
    }

    [TestCase((short)100)]
    public void Short(short value)
    {
        byte[] buffer = new byte[2];

        buffer[0] = (byte)(value << 8);
        buffer[1] = (byte)value;

        ushort decodedValue = (ushort)((buffer[0] << 8) | buffer[1]);

        Assert.That(decodedValue, Is.EqualTo(value));
    }

    [TestCase(100)]
    public void Int(int value)
    {
        byte[] buffer = new byte[4];

        buffer[0] = (byte)(value << 24);
        buffer[1] = (byte)(value << 16);
        buffer[2] = (byte)(value << 8);
        buffer[3] = (byte)value;

        int decodedValue = (buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3];

        Assert.That(decodedValue, Is.EqualTo(value));
    }

    [TestCase((uint)100)]
    public void Uint(uint value)
    {
        byte[] buffer = new byte[4];

        buffer[0] = (byte)(value << 24);
        buffer[1] = (byte)(value << 16);
        buffer[2] = (byte)(value << 8);
        buffer[3] = (byte)value;

        uint decodedValue = (uint)((buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3]);

        Assert.That(decodedValue, Is.EqualTo(value));
    }
}
