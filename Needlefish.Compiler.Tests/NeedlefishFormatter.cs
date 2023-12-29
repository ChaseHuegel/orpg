using System;

namespace Needlefish.Compiler.Tests;

internal static class NeedlefishFormatter
{
    public static void WriteBool(byte[] buffer, ref int offset, bool value)
    {
        unchecked
        {
            buffer[offset] = value ? (byte)1 : (byte)0;
            offset += 1;
        }
    }

    public static bool ReadBool(byte[] buffer, ref int offset)
    {
        unchecked
        {
            bool value = buffer[offset] != 0;
            offset += 1;
            return value;
        }
    }

    public static void WriteShort(byte[] buffer, ref int offset, short value)
    {
        unchecked
        {
            buffer[offset] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)(value);
            offset += 2;
        }
    }

    public static short ReadShort(byte[] buffer, ref int offset)
    {
        unchecked
        {
            short value = (short)(buffer[offset] | (buffer[offset + 1] << 8));
            offset += 2;
            return value;
        }
    }

    public static void WriteUShort(byte[] buffer, ref int offset, ushort value)
    {
        unchecked
        {
            buffer[offset] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)(value);
            offset += 2;
        }
    }

    public static ushort ReadUShort(byte[] buffer, ref int offset)
    {
        unchecked
        {
            ushort value = (ushort)((buffer[offset] << 8) | buffer[offset + 1]);
            offset += 2;
            return value;
        }
    }

    public static void WriteInt(byte[] buffer, ref int offset, int value)
    {
        unchecked
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value);
            offset += 4;
        }
    }

    public static int ReadInt(byte[] buffer, ref int offset)
    {
        unchecked
        {
            int value = (buffer[offset] >> 24) | (buffer[offset + 1] << 16) | (buffer[offset + 2] << 8) | (buffer[offset + 3]);
            offset += 4;
            return value;
        }
    }

    public static void WriteUInt(byte[] buffer, ref int offset, uint value)
    {
        unchecked
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value);
            offset += 4;
        }
    }

    public static uint ReadUInt(byte[] buffer, ref int offset)
    {
        unchecked
        {
            uint value = (uint)((buffer[offset] >> 24) | (buffer[offset + 1] << 16) | (buffer[offset + 2] << 16) | (buffer[offset + 3]));
            offset += 4;
            return value;
        }
    }
}
