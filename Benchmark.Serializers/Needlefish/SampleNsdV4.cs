// Code generated using nsd version 1
using Needlefish;

namespace Benchmark.Serializers.Needlefish
{
    public unsafe struct TestMessageV4
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
            unchecked
            {
                int offset = 0;

                // Int
                fixed (byte* b = &buffer[offset])
                {
                    *((ushort*)b) = Int_ID;
                }
                offset += 2;

                fixed (byte* b = &buffer[offset])
                {
                    *((int*)b) = Int;
                }
                offset += 4;

                // OptionalInt
                if (OptionalInt != null)
                {
                    fixed (byte* b = &buffer[offset])
                    {
                        *((ushort*)b) = OptionalInt_ID;
                    }
                    offset += 2;

                    buffer[offset] = 1;
                    offset += 1;

                    fixed (byte* b = &buffer[offset])
                    {
                        *((int*)b) = OptionalInt.Value;
                    }
                    offset += 4;
                }

                // Ints
                fixed (byte* b = &buffer[offset])
                {
                    *((ushort*)b) = Ints_ID;
                }
                offset += 2;

                fixed (byte* b = &buffer[offset])
                {
                    *((ushort*)b) = (ushort)(Ints?.Length ?? 0);
                }
                offset += 2;

                for (int i = 0; i < Ints?.Length; i++)
                {
                    fixed (byte* b = &buffer[offset])
                    {
                        *((int*)b) = Ints[i];
                    }
                    offset += 4;
                }

                // OptionalInts
                if (OptionalInts != null)
                {
                    fixed (byte* b = &buffer[offset])
                    {
                        *((ushort*)b) = OptionalInts_ID;
                    }
                    offset += 2;

                    buffer[offset] = 1;
                    offset += 1;

                    fixed (byte* b = &buffer[offset])
                    {
                        *((ushort*)b) = (ushort)OptionalInts.Length;
                    }
                    offset += 2;

                    for (int i = 0; i < OptionalInts?.Length; i++)
                    {
                        fixed (byte* b = &buffer[offset])
                        {
                            *((int*)b) = OptionalInts[i];
                        }
                        offset += 4;
                    }
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
