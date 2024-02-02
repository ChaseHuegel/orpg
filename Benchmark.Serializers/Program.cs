using Benchmark.Serializers.Needlefish;
using BenchmarkDotNet.Running;
using Needlefish.Compiler.Tests;

BenchmarkRunner.Run<Serialization>();

BenchmarkRunner.Run<Deserialization>();

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