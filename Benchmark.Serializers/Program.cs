using BenchmarkDotNet.Running;
using Needlefish.Compiler.Tests;

//BenchmarkRunner.Run<Serialization>();

//BenchmarkRunner.Run<Deserialization>();

var serialization = new Serialization();
var needlefish1 = serialization.Needlefish();
var needlefish2 = serialization.NeedlefishV2();
var needlefish3 = serialization.NeedlefishV3();
var needlefish4 = serialization.NeedlefishV4();
var needlefish4PreAlloc = serialization.NeedlefishV4PreAllocBuffer();
Console.WriteLine(needlefish1.Length);
Console.WriteLine(needlefish2.Length);
Console.WriteLine(needlefish3.Length);
Console.WriteLine(needlefish4.Length);
Console.WriteLine(needlefish4PreAlloc.Length);
Console.WriteLine(serialization.Protobuf().Length);
Console.WriteLine(serialization.SystemTextJson().Length);
Console.WriteLine(serialization.Newtonsoft().Length);