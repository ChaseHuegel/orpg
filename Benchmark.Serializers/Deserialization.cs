using BenchmarkDotNet.Attributes;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Text;
using NeedlefishTestMessage = Benchmark.Serializers.Needlefish.TestMessage;
using NeedlefishTestMessageV2 = Benchmark.Serializers.Needlefish.TestMessageV2;
using ProtobufTestMessage = Benchmark.Serializers.Proto.TestMessage;

namespace Needlefish.Compiler.Tests;

[MemoryDiagnoser]
public class Deserialization
{
    private readonly byte[] NeedlefishData;
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
    public NeedlefishTestMessage Newtonsoft()
    {
        return JsonConvert.DeserializeObject<NeedlefishTestMessage>(Encoding.ASCII.GetString(JsonData));
    }

    [Benchmark]
    public NeedlefishTestMessage SystemTextJson()
    {
        return System.Text.Json.JsonSerializer.Deserialize<NeedlefishTestMessage>(Encoding.ASCII.GetString(JsonData));
    }

    [Benchmark]
    public ProtobufTestMessage Protobuf()
    {
        return ProtobufTestMessage.Parser.ParseFrom(ProtobufData);
    }
}
