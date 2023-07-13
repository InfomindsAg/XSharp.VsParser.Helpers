using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using BenchmarkDotNet.Exporters.Csv;

var config = new Config()
      .WithOptions(ConfigOptions.JoinSummary)
      .WithOptions(ConfigOptions.DisableOptimizationsValidator)
      .AddDiagnoser(MemoryDiagnoser.Default);


BenchmarkRunner.Run(typeof(HashBenchmarks), config);

// new HashBenchmarks().ConsoleOutput();
// Console.ReadLine();

