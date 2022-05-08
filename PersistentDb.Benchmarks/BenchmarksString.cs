using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.EntityFrameworkCore;

namespace PersistentDb.Benchmarks;

internal record StringView(string Str);

public class BenchmarksString
{
    public static int Count { get; private set; }

    [GlobalSetup]
    public void Setup()
    {
        Count = 0;
        new LambdaFixture().SetUpContext();
    }

    [Benchmark]
    public void Reflection()
    {
        var constructor = typeof(StringView).GetConstructor(new[] { typeof(string) });
        var view = (StringView) constructor!.Invoke(new object?[] { "abc" });

        Count += view.Str.Length;
    }

    [Benchmark(Baseline = true)]
    public void DirectConstruction()
    {
        var view = new StringView("abc");
        Count += view.Str.Length;
    }
}