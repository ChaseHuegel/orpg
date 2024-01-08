using Needlefish;

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
        public int[] IntArray;
        public int[]? OptionalIntArray;

        public byte[] Serialize()
        {
            byte[] buffer = new byte[CalculateLength()];
            int offset = 0;

            //  Int
            NeedlefishFormatter.WriteHeader(buffer, ref offset, Int_ID);
            NeedlefishFormatter.Write(buffer, ref offset, Int);

            //  OptionalInt
            NeedlefishFormatter.WriteHeader(buffer, ref offset, OptionalInt_ID, isOptional: true, hasValue: OptionalInt != null);
            if (OptionalInt != null)
            {
                NeedlefishFormatter.Write(buffer, ref offset, OptionalInt.Value);
            }

            //  IntArray
            NeedlefishFormatter.WriteHeader(buffer, ref offset, IntArray_ID, isOptional: false, hasValue: IntArray != null, isArray: true, arrayLength: IntArray?.Length ?? 0);
            for (int i = 0; i < IntArray?.Length; i++)
            {
                NeedlefishFormatter.Write(buffer, ref offset, IntArray[i]);
            }

            //  OptionalIntArray
            NeedlefishFormatter.WriteHeader(buffer, ref offset, OptionalIntArray_ID, isOptional: true, hasValue: OptionalIntArray != null, isArray: true, arrayLength: OptionalIntArray?.Length ?? 0);
            for (int i = 0; i < OptionalIntArray?.Length; i++)
            {
                NeedlefishFormatter.Write(buffer, ref offset, OptionalIntArray[i]);
            }

            return buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            while (offset < buffer.Length)
            {
                ushort id = NeedlefishFormatter.ReadUShort(buffer, ref offset);
                //switch (id)
                //{
                //    case Int_ID:
                //        DecodeInt(buffer, ref offset, ref Int);
                //        break;
                //    case OptionalInt_ID:
                //        DecodeOptionalInt(buffer, ref offset, ref OptionalInt);
                //        break;
                //    case IntArray_ID:
                //        DecodeIntArray(buffer, ref offset, ref IntArray);
                //        break;
                //    case OptionalIntArray_ID:
                //        DecodeOptionalIntArray(buffer, ref offset, ref OptionalIntArray);
                //        break;
                //}
            }
        }

        private int CalculateLength()
        {
            const int boolLen = 1;
            const int shortLen = 2;
            const int intLen = 4;

            const int fieldHeaderLen = shortLen;
            const int optionalHeaderLen = boolLen;
            const int optionalFieldLen = fieldHeaderLen + optionalHeaderLen;
            const int arrayHeaderLen = shortLen;

            const int Int_MinLen = fieldHeaderLen + intLen;
            const int IntArray_MinLen = fieldHeaderLen + arrayHeaderLen;
            
            const int minLength = Int_MinLen + IntArray_MinLen;
            int length = minLength;

            if (OptionalInt.HasValue)
            {
                length += optionalFieldLen + intLen;
            }

            if (IntArray != null)
            {
                length += IntArray.Length * intLen;
            }

            if (OptionalIntArray != null)
            {
                length += optionalFieldLen + arrayHeaderLen + (OptionalIntArray.Length * intLen);
            }

            return length;
        }
    }
}