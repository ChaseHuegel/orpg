using Needlefish.Compiler.Tests;
using System.CommandLine;

var inputOption = new Option<DirectoryInfo?>(
            name: "--input",
            description: "The directory containing nsd files to use as input.");
inputOption.AddAlias("-i");

var outputOption = new Option<DirectoryInfo?>(
            name: "--output",
            description: "The directory to output generated files to.");
outputOption.AddAlias("-o");

var rootCommand = new RootCommand("The nsd compiler CLI tool.");
rootCommand.AddOption(inputOption);
rootCommand.AddOption(outputOption);

rootCommand.SetHandler(Generate, inputOption, outputOption);

return await rootCommand.InvokeAsync(args);

void Generate(DirectoryInfo? value1, DirectoryInfo? value2)
{
    string inputPath = value1?.FullName ?? Environment.CurrentDirectory;
    string outputPath = value2?.FullName ?? Environment.CurrentDirectory;

    if (!Path.IsPathFullyQualified(inputPath))
    {
        inputPath = Path.Combine(Environment.CurrentDirectory, inputPath);
    }

    if (!Path.IsPathFullyQualified(outputPath))
    {
        outputPath = Path.Combine(Environment.CurrentDirectory, outputPath);
    }

    string[] allFiles = Directory.GetFiles(inputPath, "*", SearchOption.AllDirectories);
    string[] nsdFiles = allFiles.Where(x => Path.GetExtension(x).Equals(".nsd", StringComparison.OrdinalIgnoreCase)).ToArray();

    foreach (string filePath in nsdFiles)
    {
        Console.WriteLine($"Found nsd: {Path.GetRelativePath(Environment.CurrentDirectory, filePath)}");
    }

    NsdGenerator generator = new(NsdGenerator.Version1);

    var sources = nsdFiles.Select(x => new KeyValuePair<string, string>(Path.GetFileNameWithoutExtension(x), File.ReadAllText(x))).ToArray();

    Directory.CreateDirectory(outputPath);

    foreach (var source in sources)
    {
        string result = generator.Generate(source.Key, source.Value);
   
        string filePath = Path.Combine(outputPath, source.Key + ".cs");
        File.WriteAllText(filePath, result);

        Console.WriteLine($"Generated: {Path.GetRelativePath(Environment.CurrentDirectory, filePath)}");
    }
}
