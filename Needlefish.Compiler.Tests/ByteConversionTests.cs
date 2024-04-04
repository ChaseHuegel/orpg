using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Needlefish.Compiler.Tests;

internal class ByteConversionTests
{
    private float _a = 32.1234f;

    [Test]
    public unsafe void ReverseFloatEndianness()
    {
        float result = 0f;

        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < 10000; i++)
        {
            fixed (float* ptr = &_a)
            {
                uint converted = *(uint*)ptr;
                uint dest = 0;

                dest = BinaryPrimitives.ReverseEndianness(converted);
                dest = BinaryPrimitives.ReverseEndianness(dest);

                result = *(float*)&dest;
            }
        }
        sw.Stop();

        Assert.That(result, Is.EqualTo(_a));
        Assert.That(sw.Elapsed, Is.EqualTo(TimeSpan.MinValue));
    }

    [Test]
    public unsafe void ReverseFloatEndianness2()
    {
        float result = 0f;

        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < 10000; i++)
        {
            float a = _a;
            uint converted = *(uint*)&a;
            uint dest = 0;

            dest = BinaryPrimitives.ReverseEndianness(converted);
            dest = BinaryPrimitives.ReverseEndianness(dest);

            result = *(float*)(&dest);
        }
        sw.Stop();

        Assert.That(result, Is.EqualTo(_a));
        Assert.That(sw.Elapsed, Is.EqualTo(TimeSpan.MinValue));
    }

    [Test]
    public unsafe void ReverseDoubleEndianness()
    {
        double a = 32.1234d;
        ulong converted = *(ulong*)(&a);
        ulong dest = 0;

        dest = BinaryPrimitives.ReverseEndianness(converted);
        dest = BinaryPrimitives.ReverseEndianness(dest);

        double result = *(double*)(&dest);

        Assert.That(result, Is.EqualTo(a));
    }

    [Test]
    public unsafe void PointerTraversaleLength()
    {
        int length = 0;
        byte[] buffer = new byte[100];
        fixed (byte* b = &buffer[0])
        {
            byte* end = b + 100;
            byte* offset = b;
            offset += 23;

            length = (int)(offset - b);
        }

        Assert.That(length, Is.EqualTo(23));
    }

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
