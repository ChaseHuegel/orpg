using BenchmarkDotNet.Attributes;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using NeedlefishTestMessage = Benchmark.Serializers.Needlefish.TestMessage;
using NeedlefishTestMessageV2 = Benchmark.Serializers.Needlefish.TestMessageV2;
using NeedlefishTestMessageV4 = Benchmark.Serializers.Needlefish.TestMessageV4;
using ProtobufTestMessage = Benchmark.Serializers.Proto.TestMessage;

namespace Needlefish.Compiler.Tests;

[MemoryDiagnoser]
public class Deserialization
{
    private readonly byte[] NeedlefishData;
    private readonly byte[] NeedlefishV4Data;
    private readonly byte[] JsonData;
    private readonly byte[] ProtobufData;

    public Deserialization() 
    {
        var ints = new int[] { 1, 2, 3, 4 };
        var optionalInts = new int[] { 5, 6, 7, 8 };

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

    private readonly JsonSerializerOptions JsonOptions = new()
    {
        IncludeFields = true
    };

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
}
