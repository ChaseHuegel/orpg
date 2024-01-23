using BenchmarkDotNet.Attributes;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Text;
using NeedlefishTestMessage = Benchmark.Serializers.Needlefish.TestMessage;
using NeedlefishTestMessageV2 = Benchmark.Serializers.Needlefish.TestMessageV2;
using ProtobufTestMessage = Benchmark.Serializers.Proto.TestMessage;

namespace Needlefish.Compiler.Tests;

[MemoryDiagnoser]
public class Serialization
{
    private readonly NeedlefishTestMessage NeedlefishMessage;
    private readonly NeedlefishTestMessageV2 NeedlefishMessageV2;
    private readonly ProtobufTestMessage ProtobufMessage;

    public Serialization() 
    {
        var ints = new int[] { 1, 2, 3, 4 };
        var optionalInts = new int[] { 5, 6, 7, 8 };

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

        ProtobufMessage = new ProtobufTestMessage
        {
            Int = 325,
            OptionalInt = 68,
        };
        ProtobufMessage.Ints.Add(ints);
        ProtobufMessage.OptionalInts.Add(optionalInts);
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
    public byte[] Newtonsoft()
    {
        return Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(NeedlefishMessage));
    }

    [Benchmark]
    public byte[] SystemTextJson()
    {
        return Encoding.ASCII.GetBytes(System.Text.Json.JsonSerializer.Serialize(NeedlefishMessage));
    }

    [Benchmark]
    public byte[] Protobuf()
    {
        using MemoryStream stream = new();
        ProtobufMessage.WriteTo(stream);
        return stream.ToArray();
    }
}
