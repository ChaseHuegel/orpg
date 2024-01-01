using Needlefish.Compiler.Tests;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Lexer.Tests
{
    public enum TestEnum
    {
        Val1 = 0,
        Val2 = 10,
        Val3 = 11,
        Val4 = 12
    }

    public struct TestMessage
    {
        private const ushort Int_ID = 0;
        private const ushort OptionalInt_ID = 1;
        private const ushort IntArray_ID = 2;
        private const ushort OptionalIntArray_ID = 3;

        public int Int;
        
        public int? OptionalInt;
        
        [NotNull]
        public int[] IntArray;
        
        public int[]? OptionalIntArray;

        public byte[] Serialize()
        {
            byte[] buffer = new byte[CalculateLength()];
            int offset = 0;

            EncodeInt(buffer, ref offset, Int_ID, ref Int);
            EncodeOptionalInt(buffer, ref offset, OptionalInt_ID, ref OptionalInt);
            EncodeIntArray(buffer, ref offset, IntArray_ID, ref IntArray);
            EncodeOptionalIntArray(buffer, ref offset, OptionalIntArray_ID, ref OptionalIntArray);

            return buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            while (offset < buffer.Length)
            {
                ushort id = NeedlefishFormatter.ReadUShort(buffer, ref offset);
                switch (id)
                {
                    case Int_ID:
                        DecodeInt(buffer, ref offset, ref Int);
                        break;
                    case OptionalInt_ID:
                        DecodeOptionalInt(buffer, ref offset, ref OptionalInt);
                        break;
                    case IntArray_ID:
                        DecodeIntArray(buffer, ref offset, ref IntArray);
                        break;
                    case OptionalIntArray_ID:
                        DecodeOptionalIntArray(buffer, ref offset, ref OptionalIntArray);
                        break;
                }
            }
        }

        private int CalculateLength()
        {
            const int minLength = 16;
            int length = minLength;

            if (OptionalInt.HasValue)
            {
                length += 4;
            }

            if (IntArray != null)
            {
                length += IntArray.Length * 4;
            }

            if (OptionalIntArray != null)
            {
                length += 2 + (OptionalIntArray.Length * 4);
            }

            return length;
        }

        private static void EncodeInt(byte[] buffer, ref int offset, ushort id, ref int field)
        {
            NeedlefishFormatter.WriteUShort(buffer, ref offset, id);
            NeedlefishFormatter.WriteInt(buffer, ref offset, field);
        }

        private static void EncodeOptionalInt(byte[] buffer, ref int offset, ushort id, ref int? field)
        {
            NeedlefishFormatter.WriteUShort(buffer, ref offset, id);
            NeedlefishFormatter.WriteBool(buffer, ref offset, field.HasValue);
            if (field.HasValue)
            {
                NeedlefishFormatter.WriteInt(buffer, ref offset, field.Value);
            }
        }

        private static void EncodeIntArray(byte[] buffer, ref int offset, ushort id, ref int[] field)
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

        private static void EncodeOptionalIntArray(byte[] buffer, ref int offset, ushort id, ref int[]? field)
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

        private static void DecodeInt(byte[] buffer, ref int offset, ref int field)
        {
            field = NeedlefishFormatter.ReadInt(buffer, ref offset);
        }

        private static void DecodeOptionalInt(byte[] buffer, ref int offset, ref int? field)
        {
            bool hasValue = NeedlefishFormatter.ReadBool(buffer, ref offset);
            if (hasValue)
            {
                field = NeedlefishFormatter.ReadInt(buffer, ref offset);
            }
            else
            {
                field = null;
            }
        }

        private static void DecodeIntArray(byte[] buffer, ref int offset, ref int[] field)
        {
            ushort length = NeedlefishFormatter.ReadUShort(buffer, ref offset);
            int[] array = new int[length];
            
            for (int i = 0; i < array.Length; i++)
            {
                int value = NeedlefishFormatter.ReadInt(buffer, ref offset);
                array[i] = value;
            }

            field = array;
        }

        private static void DecodeOptionalIntArray(byte[] buffer, ref int offset, ref int[]? field)
        {
            bool hasValue = NeedlefishFormatter.ReadBool(buffer, ref offset);
            if (hasValue)
            {
                DecodeIntArray(buffer, ref offset, ref field!);
            }
            else
            {
                field = null;
            }
        }
    }
}