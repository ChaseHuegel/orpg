// Code generated using nsd version 1
using Needlefish;

namespace Benchmark.Serializers.Needlefish
{
    public struct TestMessage
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
            NeedlefishFormatter.WriteHeader(buffer, ref offset, Int_ID, isOptional: false, hasValue: true, isArray: false, arrayLength: 0);
            NeedlefishFormatter.Write(buffer, ref offset, Int);

            // OptionalInt
            if (OptionalInt != null)
            {
                NeedlefishFormatter.WriteHeader(buffer, ref offset, OptionalInt_ID, isOptional: true, hasValue: true, isArray: false, arrayLength: 0);
                NeedlefishFormatter.Write(buffer, ref offset, OptionalInt.Value);
            }

            // Ints
            NeedlefishFormatter.WriteHeader(buffer, ref offset, Ints_ID, isOptional: false, hasValue: true, isArray: true, arrayLength: (ushort)(Ints?.Length ?? 0));
            for (int i = 0; i < Ints?.Length; i++)
            {
                NeedlefishFormatter.Write(buffer, ref offset, Ints[i]);
            }

            // OptionalInts
            if (OptionalInts != null)
            {
                NeedlefishFormatter.WriteHeader(buffer, ref offset, OptionalInts_ID, isOptional: true, hasValue: true, isArray: true, arrayLength: (ushort)OptionalInts.Length);
                for (int i = 0; i < OptionalInts?.Length; i++)
                {
                    NeedlefishFormatter.Write(buffer, ref offset, OptionalInts[i]);
                }
            }

        }

        public static TestMessage Deserialize(byte[] buffer)
        {
            TestMessage value = new TestMessage();
            value.Unpack(buffer);
            return value;
        }

        public void Unpack(byte[] buffer)
        {
            int offset = 0;
            while (buffer.Length - offset < 2)
            {
                ushort id = NeedlefishFormatter.ReadUShort(buffer, ref offset);
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
