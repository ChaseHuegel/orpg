namespace Needlefish.Compiler.Tests;

internal static partial class NeedlefishSerializer
{
    public static void EncodeInt(byte[] buffer, ref int offset, ushort id, ref int field)
    {
        NeedlefishFormatter.WriteUShort(buffer, ref offset, id);
        NeedlefishFormatter.WriteInt(buffer, ref offset, field);
    }

    public static void EncodeOptionalInt(byte[] buffer, ref int offset, ushort id, ref int? field)
    {
        NeedlefishFormatter.WriteUShort(buffer, ref offset, id);
        NeedlefishFormatter.WriteBool(buffer, ref offset, field.HasValue);
        if (field.HasValue)
        {
            NeedlefishFormatter.WriteInt(buffer, ref offset, field.Value);
        }
    }

    public static void EncodeIntArray(byte[] buffer, ref int offset, ushort id, ref int[] field)
    {
        NeedlefishFormatter.WriteUShort(buffer, ref offset, id);

        if (field != null && field.Length > 0)
        {
            NeedlefishFormatter.WriteUShort(buffer, ref offset, (ushort)field.Length);

            for (int i = 0; i < field.Length; i++)
            {
                NeedlefishFormatter.WriteInt(buffer, ref offset, field[i]);
            }
        }
        else
        {
            NeedlefishFormatter.WriteUShort(buffer, ref offset, 0);
        }
    }

    public static void EncodeOptionalIntArray(byte[] buffer, ref int offset, ushort id, ref int[]? field)
    {
        NeedlefishFormatter.WriteUShort(buffer, ref offset, id);

        if (field != null && field.Length > 0)
        {
            NeedlefishFormatter.WriteBool(buffer, ref offset, true);
            NeedlefishFormatter.WriteUShort(buffer, ref offset, (ushort)field.Length);

            for (int i = 0; i < field.Length; i++)
            {
                NeedlefishFormatter.WriteInt(buffer, ref offset, field[i]);
            }
        }
        else
        {
            NeedlefishFormatter.WriteBool(buffer, ref offset, false);
        }
    }
}
