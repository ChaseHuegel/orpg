using BenchmarkDotNet.Running;
using Needlefish.Compiler.Tests;

BenchmarkRunner.Run<Serialization>();

BenchmarkRunner.Run<Deserialization>();