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

            const int messageHeaderLen = byteLen;
            const int fieldHeaderLen = shortLen;
            const int optionalHeaderLen = boolLen;
            const int optionalFieldLen = fieldHeaderLen + optionalHeaderLen;
            const int arrayHeaderLen = shortLen;

            const int Int_MinLen = fieldHeaderLen + intLen;
            const int Ints_MinLen = fieldHeaderLen + arrayHeaderLen;

            const int minLength = Int_MinLen
                + Ints_MinLen;

            int length = messageHeaderLen + minLength;

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
                fixed (byte* b = &buffer[0])
                {
                    byte* offset = b;

                    //  Header
                    *((byte*)offset) = BitConverter.IsLittleEndian ? (byte)0 : (byte)1;
                    offset += 1;

                    // Int
                    *((ushort*)offset) = Int_ID;
                    offset += 2;

                    *((int*)offset) = Int;
                    offset += 4;

                    // OptionalInt
                    if (OptionalInt != null)
                    {
                        *((ushort*)offset) = OptionalInt_ID;
                        offset += 2;

                        *((byte*)offset) = 1;
                        offset += 1;

                        *((int*)offset) = OptionalInt.Value;
                        offset += 4;
                    }

                    // Ints
                    *((ushort*)offset) = Ints_ID;
                    offset += 2;

                    *((ushort*)offset) = (ushort)(Ints?.Length ?? 0);
                    offset += 2;

                    for (int i = 0; i < Ints?.Length; i++)
                    {
                        *((int*)offset) = Ints[i];
                        offset += 4;
                    }

                    // OptionalInts
                    if (OptionalInts != null)
                    {
                        *((ushort*)offset) = OptionalInts_ID;
                        offset += 2;

                        *((byte*)offset) = 1;
                        offset += 1;

                        *((ushort*)offset) = (ushort)OptionalInts.Length;
                        offset += 2;

                        for (int i = 0; i < OptionalInts?.Length; i++)
                        {
                            *((int*)offset) = OptionalInts[i];
                            offset += 4;
                        }
                    }
                }
            }
        }

        public static TestMessageV4 Deserialize(byte[] buffer)
        {
            TestMessageV4 value = new TestMessageV4();
            value.Unpack(buffer);
            return value;
        }

        public void Unpack(byte[] buffer)
        {
            unchecked
            {
                fixed (byte* b = &buffer[0])
                {
                    byte* end = b + buffer.Length;
                    byte* offset = b;

                    byte endianness = *((byte*)offset);
                    offset += 1;

                    while (offset + 2 < end)
                    {
                        ushort id = *((ushort*)offset);
                        offset += 2;

                        switch (id)
                        {
                            case Int_ID:
                                Int = *((int*)offset);
                                offset += 4;
                                break;

                            case OptionalInt_ID:
                                bool l__OptionalInt_hasValue = *((byte*)offset) == 0 ? false : true;
                                offset += 1;

                                if (l__OptionalInt_hasValue)
                                {
                                    OptionalInt = *((int*)offset);
                                    offset += 4;
                                }
                                else
                                {
                                    OptionalInt = null;
                                }
                                break;

                            case Ints_ID:
                                ushort l__Ints_length = *((ushort*)offset);
                                offset += 2;

                                if (l__Ints_length == 0)
                                {
                                    Ints = Array.Empty<int>();
                                }
                                else
                                {
                                    Ints = new int[l__Ints_length];

                                    for (int i = 0; i < l__Ints_length; i++)
                                    {
                                        Ints[i] = *((int*)offset);
                                        offset += 4;
                                    }
                                }
                                break;

                            case OptionalInts_ID:
                                bool l__OptionalInts_hasValue = *((byte*)offset) == 0 ? false : true;
                                offset += 1;

                                if (l__OptionalInts_hasValue)
                                {
                                    ushort l__OptionalInts_length = *((ushort*)offset);
                                    offset += 2;

                                    if (l__OptionalInts_length == 0)
                                    {
                                        OptionalInts = Array.Empty<int>();
                                    }
                                    else
                                    {
                                        OptionalInts = new int[l__OptionalInts_length];

                                        for (int i = 0; i < l__OptionalInts_length; i++)
                                        {
                                            OptionalInts[i] = *((int*)offset);
                                            offset += 4;
                                        }
                                    }
                                }
                                else
                                {
                                    OptionalInt = null;
                                }
                                break;
                        }
                    }
                }
            }
        }

    }

}
