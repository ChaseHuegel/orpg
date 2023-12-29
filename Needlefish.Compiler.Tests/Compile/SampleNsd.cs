using Needlefish.Compiler.Tests;

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

        public int Int;
        public int? OptionalInt;

        public byte[] Serialize()
        {
            byte[] buffer = new byte[CalculateLength()];
            int offset = 0;

            EncodeInt(buffer, ref offset, Int_ID, ref Int);
            EncodeOptionalInt(buffer, ref offset, OptionalInt_ID, ref OptionalInt);

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
                }
            }
        }

        private int CalculateLength()
        {
            const int minLength = 9;
            int length = minLength;

            if (OptionalInt.HasValue)
            {
                length += 4;
            }

            return length;
        }

        private void EncodeInt(byte[] buffer, ref int offset, ushort id, ref int field)
        {
            NeedlefishFormatter.WriteUShort(buffer, ref offset, id);
            NeedlefishFormatter.WriteInt(buffer, ref offset, field);
        }

        private void EncodeOptionalInt(byte[] buffer, ref int offset, ushort id, ref int? field)
        {
            NeedlefishFormatter.WriteUShort(buffer, ref offset, id);
            NeedlefishFormatter.WriteBool(buffer, ref offset, field.HasValue);
            if (field.HasValue)
            {
                NeedlefishFormatter.WriteInt(buffer, ref offset, field.Value);
            }
        }

        private void DecodeInt(byte[] buffer, ref int offset, ref int field)
        {
            field = NeedlefishFormatter.ReadInt(buffer, ref offset);
        }

        private void DecodeOptionalInt(byte[] buffer, ref int offset, ref int? field)
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
    }
}