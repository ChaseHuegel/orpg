using BenchmarkDotNet.Running;
using Needlefish.Compiler.Tests;

BenchmarkRunner.Run<Serialization>();

BenchmarkRunner.Run<Deserialization>();

//var serialization = new Serialization();
//Console.WriteLine(serialization.Needlefish().Length);
//Console.WriteLine(serialization.Protobuf().Length);
//Console.WriteLine(serialization.SystemTextJson().Length);
//Console.WriteLine(serialization.Newtonsoft().Length);