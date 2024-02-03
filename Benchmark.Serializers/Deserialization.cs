using BenchmarkDotNet.Attributes;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using NeedlefishTestMessage = Benchmark.Serializers.Needlefish.TestMessage;
using NeedlefishTestMessageV2 = Benchmark.Serializers.Needlefish.TestMessageV2;
using NeedlefishTestMessageV4 = Benchmark.Serializers.Needlefish.TestMessageV4;
using NeedlefishTestMessageV4Big = Benchmark.Serializers.Needlefish.TestMessageV4Big;
using ProtobufTestMessage = Benchmark.Serializers.Proto.TestMessage;
using ProtobufTestMessageBig = Benchmark.Serializers.Proto.TestMessageBig;

namespace Needlefish.Compiler.Tests;

[MemoryDiagnoser]
public class Deserialization
{
    private readonly JsonSerializerOptions JsonOptions = new()
    {
        IncludeFields = true
    };

    private readonly byte[] NeedlefishData;
    private readonly byte[] NeedlefishV4Data;
    private readonly byte[] NeedlefishV4BigData;
    private readonly byte[] NeedlefishV4HugeData;
    private readonly byte[] JsonData;
    private readonly byte[] JsonBigData;
    private readonly byte[] ProtobufData;
    private readonly byte[] ProtobufBigData;
    private readonly byte[] ProtobufHugeData;

    public Deserialization() 
    {
        var ints = new int[] { 1, 2, 3, 4 };
        var optionalInts = new int[] { 5, 6, 7, 8 };
        var strings = new string[] { "a", "quick", "brown" };
        var optionalStrings = new string[] { "fox", "jumped", "over", "the fence" };

        var needlefishMessage = new NeedlefishTestMessage
        {
            Int = 325,
            OptionalInt = 68,
            Ints = ints,
            OptionalInts = optionalInts
        };

        NeedlefishData = needlefishMessage.Serialize();

        JsonData = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(needlefishMessage));

        var needlefishMessageV4 = new NeedlefishTestMessageV4
        {
            Int = 325,
            OptionalInt = 68,
            Ints = ints,
            OptionalInts = optionalInts
        };

        NeedlefishV4Data = needlefishMessageV4.Serialize();

        var protobufMessage = new ProtobufTestMessage
        {
            Int = 325,
            OptionalInt = 68,
        };
        protobufMessage.Ints.Add(ints);
        protobufMessage.OptionalInts.Add(optionalInts);

        using MemoryStream stream = new();
        protobufMessage.WriteTo(stream);
        ProtobufData = stream.ToArray();

        var needlefishMessageV4Big = new NeedlefishTestMessageV4Big
        {
            Int = 325,
            OptionalInt = 68,
            Ints = ints,
            OptionalInts = optionalInts,
            String = "hello",
            OptionalString = "world",
            Strings = strings,
            OptionalStrings = optionalStrings,
        };

        NeedlefishV4BigData = needlefishMessageV4Big.Serialize();

        var protobufMessageBig = new ProtobufTestMessageBig
        {
            Int = 325,
            OptionalInt = 68,
            String = "hello",
            OptionalString = "world",
        };
        protobufMessageBig.Ints.Add(ints);
        protobufMessageBig.OptionalInts.Add(optionalInts);
        protobufMessageBig.Strings.Add(strings);
        protobufMessageBig.OptionalStrings.Add(optionalStrings);

        using MemoryStream streamBig = new();
        protobufMessage.WriteTo(streamBig);
        ProtobufBigData = streamBig.ToArray();

        JsonBigData = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(needlefishMessageV4Big));

        var intsHuge = new int[100_000];
        var optionalIntsHuge = new int[100_000];
        var stringsHuge = new string[50_000];
        var optionalStringsHuge = new string[50_000];

        for (int i = 0; i < intsHuge.Length; i++)
        {
            intsHuge[i] = i;
        }

        for (int i = 0; i < optionalIntsHuge.Length; i++)
        {
            optionalIntsHuge[i] = i;
        }

        for (int i = 0; i < stringsHuge.Length; i++)
        {
            stringsHuge[i] = (i * i).ToString();
        }

        for (int i = 0; i < optionalStringsHuge.Length; i++)
        {
            optionalStringsHuge[i] = (i * i).ToString();
        }

        var needlefishMessageV4Huge = new NeedlefishTestMessageV4Big
        {
            Int = 325,
            OptionalInt = 68,
            Ints = intsHuge,
            OptionalInts = optionalIntsHuge,
            String = "hello",
            OptionalString = "world",
            Strings = stringsHuge,
            OptionalStrings = optionalStringsHuge,
        };

        NeedlefishV4HugeData = new byte[needlefishMessageV4Huge.GetSize()];

        var protobufMessageHuge = new ProtobufTestMessageBig
        {
            Int = 325,
            OptionalInt = 68,
            String = "hello",
            OptionalString = "world",
        };
        protobufMessageHuge.Ints.Add(intsHuge);
        protobufMessageHuge.OptionalInts.Add(optionalIntsHuge);
        protobufMessageHuge.Strings.Add(stringsHuge);
        protobufMessageHuge.OptionalStrings.Add(optionalStringsHuge);

        using MemoryStream streamHuge = new();
        protobufMessageHuge.WriteTo(streamHuge);
        ProtobufHugeData = streamHuge.ToArray();
    }

    [Benchmark]
    public NeedlefishTestMessage Needlefish()
    {
        return NeedlefishTestMessage.Deserialize(NeedlefishData);
    }

    [Benchmark]
    public NeedlefishTestMessageV2 NeedlefishV2()
    {
        return NeedlefishTestMessageV2.Deserialize(NeedlefishData);
    }

    [Benchmark]
    public NeedlefishTestMessageV4 NeedlefishV4()
    {
        return NeedlefishTestMessageV4.Deserialize(NeedlefishV4Data);
    }

    [Benchmark]
    public NeedlefishTestMessage Newtonsoft()
    {
        return JsonConvert.DeserializeObject<NeedlefishTestMessage>(Encoding.ASCII.GetString(JsonData));
    }

    [Benchmark]
    public NeedlefishTestMessage SystemTextJson()
    {
        return System.Text.Json.JsonSerializer.Deserialize<NeedlefishTestMessage>(Encoding.ASCII.GetString(JsonData), JsonOptions);
    }

    [Benchmark]
    public ProtobufTestMessage Protobuf()
    {
        return ProtobufTestMessage.Parser.ParseFrom(ProtobufData);
    }

    [Benchmark]
    public NeedlefishTestMessageV4Big NeedlefishV4Big()
    {
        return NeedlefishTestMessageV4Big.Deserialize(NeedlefishV4BigData);
    }

    [Benchmark]
    public NeedlefishTestMessageV4Big NewtonsoftBig()
    {
        return JsonConvert.DeserializeObject<NeedlefishTestMessageV4Big>(Encoding.ASCII.GetString(JsonBigData));
    }

    [Benchmark]
    public NeedlefishTestMessageV4Big SystemTextJsonBig()
    {
        return System.Text.Json.JsonSerializer.Deserialize<NeedlefishTestMessageV4Big>(Encoding.ASCII.GetString(JsonBigData), JsonOptions);
    }

    [Benchmark]
    public ProtobufTestMessageBig ProtobufBig()
    {
        return ProtobufTestMessageBig.Parser.ParseFrom(ProtobufBigData);
    }

    [Benchmark]
    public NeedlefishTestMessageV4Big NeedlefishV4Huge()
    {
        return NeedlefishTestMessageV4Big.Deserialize(NeedlefishV4HugeData);
    }

    [Benchmark]
    public ProtobufTestMessageBig ProtobufHuge()
    {
        return ProtobufTestMessageBig.Parser.ParseFrom(ProtobufHugeData);
    }
}
