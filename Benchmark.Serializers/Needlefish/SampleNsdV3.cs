// Code generated using nsd version 1
using Google.Protobuf.WellKnownTypes;
using Microsoft.CodeAnalysis;
using Needlefish;
using Newtonsoft.Json.Linq;
using System.Buffers.Binary;

namespace Benchmark.Serializers.Needlefish
{
    public struct TestMessageV3
    {
        private const ushort Int_ID = 0;
        private const ushort OptionalInt_ID = 1;
        private const ushort Ints_ID = 2;
        private const ushort OptionalInts_ID = 3;

        public int Int;
        public int? OptionalInt;
        public int[] Ints;
        public int[]? OptionalInts;

        public int GetSize()
        {
            const int byteLen = 1;
            const int boolLen = 1;
            const int shortLen = 2;
            const int intLen = 4;
            const int floatLen = 4;
            const int longLen = 8;
            const int doubleLen = 8;

            const int fieldHeaderLen = shortLen;
            const int optionalHeaderLen = boolLen;
            const int optionalFieldLen = fieldHeaderLen + optionalHeaderLen;
            const int arrayHeaderLen = shortLen;

            const int Int_MinLen = fieldHeaderLen + intLen;
            const int Ints_MinLen = fieldHeaderLen + arrayHeaderLen;

            const int minLength = Int_MinLen
                + Ints_MinLen;

            int length = minLength;

            if (OptionalInt != null)
            {
                length += optionalFieldLen + intLen;
            }

            if (Ints != null)
            {
                length += Ints.Length * intLen;
            }

            if (OptionalInts != null)
            {
                length += optionalFieldLen + arrayHeaderLen + OptionalInts.Length * intLen;
            }

            return length;
        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[GetSize()];
            SerializeInto(buffer);
            return buffer;
        }

        public void SerializeInto(byte[] buffer)
        {
            int offset = 0;

            // Int
            BinaryPrimitives.WriteUInt16LittleEndian(new Span<byte>(buffer, offset, buffer.Length - offset), Int_ID);
            offset += 2;
            BinaryPrimitives.WriteInt32LittleEndian(new Span<byte>(buffer, offset, buffer.Length - offset), Int);
            offset += 4;

            // OptionalInt
            if (OptionalInt != null)
            {
                BinaryPrimitives.WriteUInt16LittleEndian(new Span<byte>(buffer, offset, buffer.Length - offset), OptionalInt_ID);
                offset += 2;
                buffer[offset] = 1;
                offset += 1;

                BinaryPrimitives.WriteInt32LittleEndian(new Span<byte>(buffer, offset, buffer.Length - offset), OptionalInt.Value);
                offset += 4;
            }

            // Ints
            BinaryPrimitives.WriteUInt16LittleEndian(new Span<byte>(buffer, offset, buffer.Length - offset), Ints_ID);
            offset += 2;
            BinaryPrimitives.WriteUInt16LittleEndian(new Span<byte>(buffer, offset, buffer.Length - offset), (ushort)(Ints?.Length ?? 0));
            offset += 2;
            for (int i = 0; i < Ints?.Length; i++)
            {
                BinaryPrimitives.WriteInt32LittleEndian(new Span<byte>(buffer, offset, buffer.Length - offset), Ints[i]);
                offset += 4;
            }

            // OptionalInts
            if (OptionalInts != null)
            {
                BinaryPrimitives.WriteUInt16LittleEndian(new Span<byte>(buffer, offset, buffer.Length - offset), OptionalInt_ID);
                offset += 2;
                buffer[offset] = 1;
                offset += 1;
                BinaryPrimitives.WriteUInt16LittleEndian(new Span<byte>(buffer, offset, buffer.Length - offset), (ushort)OptionalInts.Length);
                offset += 2;

                for (int i = 0; i < OptionalInts?.Length; i++)
                {
                    BinaryPrimitives.WriteInt32LittleEndian(new Span<byte>(buffer, offset, buffer.Length - offset), OptionalInts[i]);
                    offset += 4;
                }
            }

        }

        public static TestMessageV2 Deserialize(byte[] buffer)
        {
            TestMessageV2 value = new TestMessageV2();
            value.Unpack(buffer);
            return value;
        }

        public void Unpack(byte[] buffer)
        {
            int offset = 0;
            while (buffer.Length - offset < 2)
            {
                ushort id = NeedlefishFormatterV2.ReadUShort(buffer, ref offset);
                switch (id)
                {
                    case Int_ID:
                        break;
                    case OptionalInt_ID:
                        break;
                    case Ints_ID:
                        break;
                    case OptionalInts_ID:
                        break;
                }
            }
        }

    }

}
