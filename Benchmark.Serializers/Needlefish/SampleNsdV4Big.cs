// Code generated using nsd version 1
using Needlefish;
using System.Buffers.Binary;

namespace Benchmark.Serializers.Needlefish
{
    public unsafe struct TestMessageV4Big
    {
        private const ushort Int_ID = 0;
        private const ushort OptionalInt_ID = 1;
        private const ushort Ints_ID = 2;
        private const ushort OptionalInts_ID = 3;
        private const ushort String_ID = 4;
        private const ushort OptionalString_ID = 5;
        private const ushort Strings_ID = 6;
        private const ushort OptionalStrings_ID = 7;

        public int Int;
        public int? OptionalInt;
        public int[] Ints;
        public int[]? OptionalInts;

        public string String;
        public string? OptionalString;
        public string[] Strings;
        public string[]? OptionalStrings;

        public int GetSize()
        {
            const int byteLen = 1;
            const int boolLen = 1;
            const int shortLen = 2;
            const int charLen = 2;
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

            const int String_MinLen = fieldHeaderLen + arrayHeaderLen;
            const int Strings_MinLen = fieldHeaderLen + arrayHeaderLen;

            const int minLength = Int_MinLen
                + Ints_MinLen
                + String_MinLen
                + Strings_MinLen;

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

            if (String != null)
            {
                length += String.Length * charLen;
            }

            if (OptionalString != null)
            {
                length += optionalFieldLen + arrayHeaderLen + OptionalString.Length * charLen;
            }

            if (Strings != null)
            {
                for (int i = 0; i < Strings.Length; i++)
                {
                    length += arrayHeaderLen + Strings[i].Length * charLen;
                }
            }

            if (OptionalStrings != null)
            {
                length += optionalFieldLen + arrayHeaderLen;

                for (int i = 0; i < OptionalStrings.Length; i++)
                {
                    length += arrayHeaderLen + OptionalStrings[i].Length * charLen;
                }
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

                    // Int
                    *((ushort*)offset) = BitConverter.IsLittleEndian ? Int_ID : BinaryPrimitives.ReverseEndianness(Int_ID);
                    offset += 2;

                    *((int*)offset) = BitConverter.IsLittleEndian ? Int : BinaryPrimitives.ReverseEndianness(Int);
                    offset += 4;

                    // OptionalInt
                    if (OptionalInt != null)
                    {
                        *((ushort*)offset) = BitConverter.IsLittleEndian ? OptionalInt_ID : BinaryPrimitives.ReverseEndianness(OptionalInt_ID);
                        offset += 2;

                        *((byte*)offset) = 1;
                        offset += 1;

                        *((int*)offset) = BitConverter.IsLittleEndian ? OptionalInt.Value : BinaryPrimitives.ReverseEndianness(OptionalInt.Value);
                        offset += 4;
                    }

                    // Ints
                    *((ushort*)offset) = BitConverter.IsLittleEndian ? Ints_ID : BinaryPrimitives.ReverseEndianness(Ints_ID);
                    offset += 2;

                    *((ushort*)offset) = BitConverter.IsLittleEndian ? (ushort)(Ints?.Length ?? 0) : BinaryPrimitives.ReverseEndianness((ushort)(Ints?.Length ?? 0));
                    offset += 2;

                    for (int i = 0; i < Ints?.Length; i++)
                    {
                        *((int*)offset) = BitConverter.IsLittleEndian ? Ints[i] : BinaryPrimitives.ReverseEndianness(Ints[i]);
                        offset += 4;
                    }

                    // OptionalInts
                    if (OptionalInts != null)
                    {
                        *((ushort*)offset) = BitConverter.IsLittleEndian ? OptionalInts_ID : BinaryPrimitives.ReverseEndianness(OptionalInts_ID);
                        offset += 2;

                        *((byte*)offset) = 1;
                        offset += 1;

                        *((ushort*)offset) = BitConverter.IsLittleEndian ? (ushort)OptionalInts.Length : BinaryPrimitives.ReverseEndianness((ushort)OptionalInts.Length);
                        offset += 2;

                        for (int i = 0; i < OptionalInts?.Length; i++)
                        {
                            *((int*)offset) = BitConverter.IsLittleEndian ? OptionalInts[i] : BinaryPrimitives.ReverseEndianness(OptionalInts[i]);
                            offset += 4;
                        }
                    }

                    // String
                    *((ushort*)offset) = BitConverter.IsLittleEndian ? String_ID : BinaryPrimitives.ReverseEndianness(String_ID);
                    offset += 2;

                    if (String != null)
                    {
                        *((ushort*)offset) = BitConverter.IsLittleEndian ? (ushort)String.Length : BinaryPrimitives.ReverseEndianness((ushort)String.Length);
                        offset += 2;

                        if (String.Length > 0)
                        {
                            for (int i = 0; i < String.Length; i++)
                            {
                                *((char*)offset) = BitConverter.IsLittleEndian ? String[i] : (char)BinaryPrimitives.ReverseEndianness(String[i]);
                                offset += 2;
                            }
                        }
                    }
                    else
                    {
                        *((ushort*)offset) = 0;
                        offset += 2;
                    }

                    // OptionalString
                    if (OptionalString != null)
                    {
                        *((ushort*)offset) = BitConverter.IsLittleEndian ? OptionalString_ID : BinaryPrimitives.ReverseEndianness(OptionalString_ID);
                        offset += 2;

                        *((byte*)offset) = 1;
                        offset += 1;

                        *((ushort*)offset) = BitConverter.IsLittleEndian ? (ushort)OptionalString.Length : BinaryPrimitives.ReverseEndianness((ushort)OptionalString.Length);
                        offset += 2;

                        if (OptionalString.Length > 0)
                        {
                            for (int i = 0; i < OptionalString.Length; i++)
                            {
                                *((char*)offset) = BitConverter.IsLittleEndian ? OptionalString[i] : (char)BinaryPrimitives.ReverseEndianness(OptionalString[i]);
                                offset += 2;
                            }
                        }
                    }

                    //  Strings
                    *((ushort*)offset) = BitConverter.IsLittleEndian ? Strings_ID : BinaryPrimitives.ReverseEndianness(Strings_ID);
                    offset += 2;

                    *((ushort*)offset) = BitConverter.IsLittleEndian ? (ushort)(Strings?.Length ?? 0) : BinaryPrimitives.ReverseEndianness((ushort)(Strings?.Length ?? 0));
                    offset += 2;

                    for (int i = 0; i < Strings?.Length; i++)
                    {
                        string item = Strings[i];

                        *((ushort*)offset) = BitConverter.IsLittleEndian ? (ushort)(item?.Length ?? 0) : BinaryPrimitives.ReverseEndianness((ushort)(item?.Length ?? 0));
                        offset += 2;

                        if (item != null && item.Length > 0)
                        {
                            for (int n = 0; n < item.Length; n++)
                            {
                                *((char*)offset) = BitConverter.IsLittleEndian ? item[n] : (char)BinaryPrimitives.ReverseEndianness(item[n]);
                                offset += 2;
                            }
                        }
                    }

                    // OptionalStrings
                    if (OptionalStrings != null)
                    {
                        *((ushort*)offset) = BitConverter.IsLittleEndian ? OptionalStrings_ID : BinaryPrimitives.ReverseEndianness(OptionalStrings_ID);
                        offset += 2;

                        *((byte*)offset) = 1;
                        offset += 1;

                        *((ushort*)offset) = BitConverter.IsLittleEndian ? (ushort)OptionalStrings.Length : BinaryPrimitives.ReverseEndianness((ushort)OptionalStrings.Length);
                        offset += 2;

                        if (OptionalStrings.Length > 0)
                        {
                            for (int i = 0; i < OptionalStrings.Length; i++)
                            {
                                string item = OptionalStrings[i];

                                *((ushort*)offset) = BitConverter.IsLittleEndian ? (ushort)(item?.Length ?? 0) : BinaryPrimitives.ReverseEndianness((ushort)(item?.Length ?? 0));
                                offset += 2;

                                if (item != null && item.Length > 0)
                                {
                                    for (int n = 0; n < item.Length; n++)
                                    {
                                        *((char*)offset) = BitConverter.IsLittleEndian ? item[n] : (char)BinaryPrimitives.ReverseEndianness(item[n]);
                                        offset += 2;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static TestMessageV4Big Deserialize(byte[] buffer)
        {
            TestMessageV4Big value = new TestMessageV4Big();
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

                    while (offset + 2 < end)
                    {
                        ushort id = BitConverter.IsLittleEndian ? *((ushort*)offset) : BinaryPrimitives.ReverseEndianness(*((ushort*)offset));
                        offset += 2;

                        switch (id)
                        {
                            case Int_ID:
                                Int = BitConverter.IsLittleEndian ? *((int*)offset) : BinaryPrimitives.ReverseEndianness(*((int*)offset));
                                offset += 4;
                                break;

                            case OptionalInt_ID:
                                bool l__OptionalInt_hasValue = *((byte*)offset) == 0 ? false : true;
                                offset += 1;

                                if (l__OptionalInt_hasValue)
                                {
                                    OptionalInt = BitConverter.IsLittleEndian ? *((int*)offset) : BinaryPrimitives.ReverseEndianness(*((int*)offset));
                                    offset += 4;
                                }
                                else
                                {
                                    OptionalInt = null;
                                }
                                break;

                            case Ints_ID:
                                ushort l__Ints_length = BitConverter.IsLittleEndian ? *((ushort*)offset) : BinaryPrimitives.ReverseEndianness(*((ushort*)offset));
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
                                        Ints[i] = BitConverter.IsLittleEndian ? *((int*)offset) : BinaryPrimitives.ReverseEndianness(*((int*)offset));
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
                                            OptionalInts[i] = BitConverter.IsLittleEndian ? *((int*)offset) : BinaryPrimitives.ReverseEndianness(*((int*)offset));
                                            offset += 4;
                                        }
                                    }
                                }
                                else
                                {
                                    OptionalInt = null;
                                }
                                break;

                            case String_ID:
                                ushort l__String_length = BitConverter.IsLittleEndian ? *((ushort*)offset) : BinaryPrimitives.ReverseEndianness(*((ushort*)offset));
                                offset += 2;

                                if (l__String_length == 0)
                                {
                                    String = "";
                                }
                                else
                                {
                                    char* chars = (char*)offset;
                                    if (!BitConverter.IsLittleEndian)
                                    {
                                        for (int n = 0; n < l__String_length; n++)
                                        {
                                            *((ushort*)chars) = BinaryPrimitives.ReverseEndianness(*((ushort*)chars));
                                            chars += 2;
                                        }
                                    }

                                    String = new string(chars, 0, l__String_length);
                                    offset += 2 * l__String_length;
                                }
                                break;

                            case Strings_ID:
                                ushort l__Strings_length = BitConverter.IsLittleEndian ? *((ushort*)offset) : BinaryPrimitives.ReverseEndianness(*((ushort*)offset));
                                offset += 2;

                                if (l__Strings_length == 0)
                                {
                                    Strings = Array.Empty<string>();
                                }
                                else
                                {
                                    Strings = new string[l__Strings_length];

                                    for (int i = 0; i < l__Strings_length; i++)
                                    {
                                        ushort l__Strings_i_length = BitConverter.IsLittleEndian ? *((ushort*)offset) : BinaryPrimitives.ReverseEndianness(*((ushort*)offset));
                                        offset += 2;

                                        char* chars = (char*)offset;
                                        if (!BitConverter.IsLittleEndian)
                                        {
                                            for (int n = 0; n < l__Strings_i_length; n++)
                                            {
                                                *((ushort*)chars) = BinaryPrimitives.ReverseEndianness(*((ushort*)chars));
                                                chars += 2;
                                            }
                                        }

                                        Strings[i] = new string(chars, 0, l__Strings_i_length);
                                        offset += 2 * l__Strings_i_length;
                                    }
                                }
                                break;

                            case OptionalString_ID:
                                bool l__OptionalString_hasValue = *((byte*)offset) == 0 ? false : true;
                                offset += 1;

                                if (l__OptionalString_hasValue)
                                {
                                    ushort l__OptionalString_length = BitConverter.IsLittleEndian ? *((ushort*)offset) : BinaryPrimitives.ReverseEndianness(*((ushort*)offset));
                                    offset += 2;

                                    if (l__OptionalString_length == 0)
                                    {
                                        OptionalString = "";
                                    }
                                    else
                                    {
                                        char* chars = (char*)offset;
                                        if (!BitConverter.IsLittleEndian)
                                        {
                                            for (int n = 0; n < l__OptionalString_length; n++)
                                            {
                                                *((ushort*)chars) = BinaryPrimitives.ReverseEndianness(*((ushort*)chars));
                                                chars += 2;
                                            }
                                        }

                                        OptionalString = new string(chars, 0, l__OptionalString_length);
                                        offset += 2 * l__OptionalString_length;
                                    }
                                }
                                else
                                {
                                    OptionalInt = null;
                                }
                                break;

                            case OptionalStrings_ID:
                                bool l__OptionalStrings_hasValue = *((byte*)offset) == 0 ? false : true;
                                offset += 1;

                                if (l__OptionalStrings_hasValue)
                                {
                                    ushort l__OptionalStrings_length = BitConverter.IsLittleEndian ? *((ushort*)offset) : BinaryPrimitives.ReverseEndianness(*((ushort*)offset));
                                    offset += 2;

                                    if (l__OptionalStrings_length == 0)
                                    {
                                        OptionalStrings = Array.Empty<string>();
                                    }
                                    else
                                    {
                                        OptionalStrings = new string[l__OptionalStrings_length];

                                        for (int i = 0; i < l__OptionalStrings_length; i++)
                                        {
                                            ushort l__OptionalStrings_i_length = BitConverter.IsLittleEndian ? *((ushort*)offset) : BinaryPrimitives.ReverseEndianness(*((ushort*)offset));
                                            offset += 2;

                                            char* chars = (char*)offset;
                                            if (!BitConverter.IsLittleEndian)
                                            {
                                                for (int n = 0; n < l__OptionalStrings_i_length; n++)
                                                {
                                                    *((ushort*)chars) = BinaryPrimitives.ReverseEndianness(*((ushort*)chars));
                                                    chars += 2;
                                                }
                                            }

                                            OptionalStrings[i] = new string(chars, 0, l__OptionalStrings_i_length);
                                            offset += 2 * l__OptionalStrings_i_length;
                                        }
                                    }
                                }
                                else
                                {
                                    OptionalStrings = null;
                                }
                                break;
                        }
                    }
                }
            }
        }

    }

}
