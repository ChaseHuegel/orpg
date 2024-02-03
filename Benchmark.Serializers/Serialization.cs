using BenchmarkDotNet.Attributes;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using NeedlefishTestMessage = Benchmark.Serializers.Needlefish.TestMessage;
using NeedlefishTestMessageV2 = Benchmark.Serializers.Needlefish.TestMessageV2;
using NeedlefishTestMessageV3 = Benchmark.Serializers.Needlefish.TestMessageV3;
using NeedlefishTestMessageV4 = Benchmark.Serializers.Needlefish.TestMessageV4;
using NeedlefishTestMessageV4Big = Benchmark.Serializers.Needlefish.TestMessageV4Big;
using ProtobufTestMessage = Benchmark.Serializers.Proto.TestMessage;
using ProtobufTestMessageBig = Benchmark.Serializers.Proto.TestMessageBig;

namespace Needlefish.Compiler.Tests;

[MemoryDiagnoser]
public class Serialization
{
    private readonly JsonSerializerOptions JsonOptions = new()
    {
        IncludeFields = true
    };

    private readonly NeedlefishTestMessage NeedlefishMessage;
    private readonly NeedlefishTestMessageV2 NeedlefishMessageV2;
    private readonly NeedlefishTestMessageV3 NeedlefishMessageV3;
    private readonly NeedlefishTestMessageV4 NeedlefishMessageV4;
    private readonly NeedlefishTestMessageV4Big NeedlefishMessageV4Big;
    private readonly NeedlefishTestMessageV4Big NeedlefishMessageV4Huge;
    private readonly ProtobufTestMessage ProtobufMessage;
    private readonly ProtobufTestMessageBig ProtobufMessageBig;
    private readonly ProtobufTestMessageBig ProtobufMessageHuge;
    private readonly byte[] NeedlefishMessageV4Buffer;
    private readonly byte[] NeedlefishMessageV4BigBuffer;
    private readonly byte[] NeedlefishMessageV4HugeBuffer;
    private readonly MemoryStream ProtobufMessageBuffer;
    private readonly MemoryStream ProtobufMessageBigBuffer;
    private readonly MemoryStream ProtobufMessageHugeBuffer;

    public Serialization() 
    {
        var ints = new int[] { 1, 2, 3, 4 };
        var optionalInts = new int[] { 5, 6, 7, 8 };
        var strings = new string[] { "a", "quick", "brown" };
        var optionalStrings = new string[] { "fox", "jumped", "over", "the fence" };

        NeedlefishMessage = new NeedlefishTestMessage
        {
            Int = 325,
            OptionalInt = 68,
            Ints = ints,
            OptionalInts = optionalInts
        };

        NeedlefishMessageV2 = new NeedlefishTestMessageV2
        {
            Int = 325,
            OptionalInt = 68,
            Ints = ints,
            OptionalInts = optionalInts
        };

        NeedlefishMessageV3 = new NeedlefishTestMessageV3
        {
            Int = 325,
            OptionalInt = 68,
            Ints = ints,
            OptionalInts = optionalInts
        };

        NeedlefishMessageV4 = new NeedlefishTestMessageV4
        {
            Int = 325,
            OptionalInt = 68,
            Ints = ints,
            OptionalInts = optionalInts
        };

        NeedlefishMessageV4Buffer = new byte[NeedlefishMessageV4.GetSize()];

        ProtobufMessage = new ProtobufTestMessage
        {
            Int = 325,
            OptionalInt = 68,
        };
        ProtobufMessage.Ints.Add(ints);
        ProtobufMessage.OptionalInts.Add(optionalInts);

        NeedlefishMessageV4Big = new NeedlefishTestMessageV4Big
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

        NeedlefishMessageV4BigBuffer = new byte[NeedlefishMessageV4Big.GetSize()];

        ProtobufMessageBig = new ProtobufTestMessageBig
        {
            Int = 325,
            OptionalInt = 68,
            String = "hello",
            OptionalString = "world",
        };
        ProtobufMessageBig.Ints.Add(ints);
        ProtobufMessageBig.OptionalInts.Add(optionalInts);
        ProtobufMessageBig.Strings.Add(strings);
        ProtobufMessageBig.OptionalStrings.Add(optionalStrings);

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

        NeedlefishMessageV4Huge = new NeedlefishTestMessageV4Big
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

        NeedlefishMessageV4HugeBuffer = new byte[NeedlefishMessageV4Huge.GetSize()];

        ProtobufMessageHuge = new ProtobufTestMessageBig
        {
            Int = 325,
            OptionalInt = 68,
            String = "hello",
            OptionalString = "world",
        };
        ProtobufMessageHuge.Ints.Add(intsHuge);
        ProtobufMessageHuge.OptionalInts.Add(optionalIntsHuge);
        ProtobufMessageHuge.Strings.Add(stringsHuge);
        ProtobufMessageHuge.OptionalStrings.Add(optionalStringsHuge);

        ProtobufMessageBuffer = new MemoryStream(ProtobufMessage.CalculateSize());
        ProtobufMessageBigBuffer = new MemoryStream(ProtobufMessageBig.CalculateSize());
        ProtobufMessageHugeBuffer = new MemoryStream(ProtobufMessageHuge.CalculateSize());
    }

    [Benchmark]
    public byte[] Needlefish()
    {
        return NeedlefishMessage.Serialize();
    }

    [Benchmark]
    public byte[] NeedlefishV2()
    {
        return NeedlefishMessageV2.Serialize();
    }

    [Benchmark]
    public byte[] NeedlefishV3()
    {
        return NeedlefishMessageV3.Serialize();
    }

    [Benchmark]
    public byte[] NeedlefishV4()
    {
        return NeedlefishMessageV4.Serialize();
    }

    [Benchmark]
    public byte[] NeedlefishV4PreAllocBuffer()
    {
        NeedlefishMessageV4.SerializeInto(NeedlefishMessageV4Buffer);
        return NeedlefishMessageV4Buffer;
    }

    [Benchmark]
    public byte[] Newtonsoft()
    {
        return Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(NeedlefishMessage));
    }

    [Benchmark]
    public byte[] SystemTextJson()
    {
        return Encoding.ASCII.GetBytes(System.Text.Json.JsonSerializer.Serialize(NeedlefishMessage, JsonOptions));
    }

    [Benchmark]
    public byte[] Protobuf()
    {
        using MemoryStream stream = new();
        ProtobufMessage.WriteTo(stream);
        return stream.ToArray();
    }

    [Benchmark]
    public MemoryStream ProtobufPreAllocBuffer()
    {
        ProtobufMessageBuffer.Position = 0;
        ProtobufMessageBig.WriteTo(ProtobufMessageBuffer);
        return ProtobufMessageBuffer;
    }

    [Benchmark]
    public byte[] NeedlefishV4Big()
    {
        return NeedlefishMessageV4Big.Serialize();
    }

    [Benchmark]
    public byte[] NeedlefishV4BigPreAllocBuffer()
    {
        NeedlefishMessageV4Big.SerializeInto(NeedlefishMessageV4BigBuffer);
        return NeedlefishMessageV4BigBuffer;
    }

    [Benchmark]
    public byte[] NewtonsoftBig()
    {
        return Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(NeedlefishMessageV4Big));
    }

    [Benchmark]
    public byte[] SystemTextJsonBig()
    {
        return Encoding.ASCII.GetBytes(System.Text.Json.JsonSerializer.Serialize(NeedlefishMessageV4Big, JsonOptions));
    }

    [Benchmark]
    public byte[] ProtobufBig()
    {
        using MemoryStream stream = new();
        ProtobufMessageBig.WriteTo(stream);
        return stream.ToArray();
    }

    [Benchmark]
    public MemoryStream ProtobufBigPreAllocBuffer()
    {
        ProtobufMessageBigBuffer.Position = 0;
        ProtobufMessageBig.WriteTo(ProtobufMessageBigBuffer);
        return ProtobufMessageBigBuffer;
    }

    [Benchmark]
    public byte[] NeedlefishV4Huge()
    {
        return NeedlefishMessageV4Huge.Serialize();
    }

    [Benchmark]
    public byte[] NeedlefishV4HugePreAllocBuffer()
    {
        NeedlefishMessageV4Huge.SerializeInto(NeedlefishMessageV4HugeBuffer);
        return NeedlefishMessageV4HugeBuffer;
    }

    [Benchmark]
    public byte[] ProtobufHuge()
    {
        using MemoryStream stream = new();
        ProtobufMessageHuge.WriteTo(stream);
        return stream.ToArray();
    }

    [Benchmark]
    public MemoryStream ProtobufHugePreAllocBuffer()
    {
        ProtobufMessageHugeBuffer.Position = 0;
        ProtobufMessageHuge.WriteTo(ProtobufMessageHugeBuffer);
        return ProtobufMessageHugeBuffer;
    }
}
