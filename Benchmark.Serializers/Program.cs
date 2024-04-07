using Benchmark.Serializers.Needlefish;
using BenchmarkDotNet.Running;
using Needlefish.Compiler.Tests;

//BenchmarkRunner.Run<Serialization>();

//BenchmarkRunner.Run<Deserialization>();

//var ints = new int[] { 1, 2, 3, 4 };
//var optionalInts = new int[] { 5, 6, 7, 8 };

//var needlefishMessageV4 = new TestMessageV4Big
//{
//    Int = 325,
//    OptionalInt = 68,
//    Ints = ints,
//    OptionalInts = optionalInts,
//    String = "hello",
//    OptionalString = "world",
//    Strings = new string[] { "a", "quick", "brown" },
//    OptionalStrings = new string[] { "fox", "jumped", "over", "the fence" },
//};

//var data = needlefishMessageV4.Serialize();

//TestMessageV4Big deserializedMessage = TestMessageV4Big.Deserialize(data);

//Console.WriteLine(deserializedMessage.Int);
//Console.WriteLine(deserializedMessage.OptionalInt);
//Console.WriteLine(string.Join(',', deserializedMessage.Ints));
//Console.WriteLine(string.Join(',', deserializedMessage.OptionalInts!));
//Console.WriteLine(deserializedMessage.String);
//Console.WriteLine(deserializedMessage.OptionalString);
//Console.WriteLine(string.Join(',', deserializedMessage.Strings));
//Console.WriteLine(string.Join(',', deserializedMessage.OptionalStrings!));

var message = new Lexer.Tests.TestMessage
{
    Content = "quick",
    Body = "brown",
    FloAT = 1.234f,
    Double = 5.678d,
    Long = 1337,
    uLong = 1338,
    Ulong = 1339,
    Short = 1340,
    UShort = 1341,
    Bool = true,
    Byte = 128,
    Bytes = new byte[]{1, 5, 12, 39},
    OptionalStrings = new string[] { "fox", "jumped", "over", "the fence" },
    Int = 1342,
    OptionalInt = 1343,
    Ints = new int[] {1344, 1345, 1355},
    OptionalInts = new int[] { 1346, 1347 },
    UInt = 1348,
    OptionalUInt = 1349,
    UInts = new uint[] { 1350, 1351 },
    OptionalUInts = new uint[] { 1352, 1353 },
    Enum = Lexer.Tests.TestEnum.Val2,
    OptionalEnum = Lexer.Tests.TestEnum.Val3,
    Enums = new Lexer.Tests.TestEnum[] { Lexer.Tests.TestEnum.Val3, Lexer.Tests.TestEnum.Val2, Lexer.Tests.TestEnum.Val1 },
    OptionalEnums = new Lexer.Tests.TestEnum[] { Lexer.Tests.TestEnum.Val3, Lexer.Tests.TestEnum.Val2, Lexer.Tests.TestEnum.Val1 },
    Submessage = new Lexer.Tests.Submessage { OptionalInt = 1 },
    OptionalSubmessage = new Lexer.Tests.Submessage { OptionalInt = 2 },
    Submessages = new Lexer.Tests.Submessage[] { new Lexer.Tests.Submessage { OptionalInt = 3 }, new Lexer.Tests.Submessage { OptionalInt = 4 } },
    OptionalSubmessages = new Lexer.Tests.Submessage[] { new Lexer.Tests.Submessage { OptionalInt = 5 }, new Lexer.Tests.Submessage { OptionalInt = 6 } },
};

var data = message.Serialize();

Lexer.Tests.TestMessage deserializedMessage = Lexer.Tests.TestMessage.Deserialize(data, 0, data.Length);

Console.WriteLine(deserializedMessage.Equals(data));

//for (int i = 0; i < data.Length; i++)
//{
//    data[i] = 0;
//}

//Console.WriteLine(deserializedMessage.String);

//data = needlefishMessageV4.Serialize();

//deserializedMessage = TestMessageV4Big.Deserialize(data);

//Console.WriteLine(deserializedMessage.String);

//var ints = new int[] { 1, 2, 3, 4 };
//var optionalInts = new int[] { 5, 6, 7, 8 };

//var needlefishMessageV4 = new TestMessageV4Big
//{
//    Int = 325,
//    OptionalInt = 68,
//    Ints = ints,
//    OptionalInts = optionalInts,
//    String = "hello",
//    OptionalString = "world",
//    Strings = new string[] { "a", "quick", "brown" },
//    OptionalStrings = new string[] { "fox", "jumped", "over", "the fence" },
//};

//var data = needlefishMessageV4.Serialize();

//TestMessageV4Big deserializedMessage = TestMessageV4Big.Deserialize(data);

//Console.WriteLine(deserializedMessage.String);

//for (int i = 0; i < data.Length; i++)
//{
//    data[i] = 0;
//}

//Console.WriteLine(deserializedMessage.String);

//data = needlefishMessageV4.Serialize();

//deserializedMessage = TestMessageV4Big.Deserialize(data);

//Console.WriteLine(deserializedMessage.String);

//var ints = new int[] { 1, 2, 3, 4 };
//var optionalInts = new int[] { 5, 6, 7, 8 };

//var needlefishMessageV4 = new TestMessageV4Big
//{
//    Int = 325,
//    OptionalInt = 68,
//    Ints = ints,
//    OptionalInts = optionalInts,
//    String = "hello",
//    OptionalString = "world",
//    Strings = new string[] { "a", "quick", "brown" },
//    OptionalStrings = new string[] { "fox", "jumped", "over", "the fence" },
//};

//var data = needlefishMessageV4.Serialize();

//TestMessageV4Big deserializedMessage = TestMessageV4Big.Deserialize(data);

//Console.WriteLine(deserializedMessage.String);

//var serialization = new Serialization();
//var needlefishV4Huge = serialization.NeedlefishV4Huge();
//var protobufHuge = serialization.ProtobufHuge();
//Console.WriteLine(needlefishV4Huge.Length);
//Console.WriteLine(protobufHuge.Length);
//var needlefishV4Big = serialization.NeedlefishV4Big();
//var jsonBig = serialization.SystemTextJsonBig();
//var protobufBig = serialization.ProtobufBig();
//Console.WriteLine(needlefishV4Big.Length);
//Console.WriteLine(jsonBig.Length);
//Console.WriteLine(protobufBig.Length);
//var systemTextJson = serialization.SystemTextJson();
//var needlefish1 = serialization.Needlefish();
//var needlefish2 = serialization.NeedlefishV2();
//var needlefish3 = serialization.NeedlefishV3();
//var needlefish4 = serialization.NeedlefishV4();
//var needlefish4PreAlloc = serialization.NeedlefishV4PreAllocBuffer();

//Console.WriteLine(BitConverter.IsLittleEndian);
//Console.WriteLine(needlefish1.SequenceEqual(needlefish4));
//Console.WriteLine(string.Join(' ', needlefish1));
//Console.WriteLine(string.Join(' ', needlefish4));

//var message = TestMessageV4.Deserialize(needlefish4);

//Console.WriteLine(message);

//Console.WriteLine(needlefish1.Length);
//Console.WriteLine(needlefish2.Length);
//Console.WriteLine(needlefish3.Length);
//Console.WriteLine(needlefish4.Length);
//Console.WriteLine(needlefish4PreAlloc.Length);
//Console.WriteLine(serialization.Protobuf().Length);
//Console.WriteLine(serialization.SystemTextJson().Length);
//Console.WriteLine(serialization.Newtonsoft().Length);