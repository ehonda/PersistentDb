using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace PersistentDb.Benchmarks;

public class Benchmarks
{
    private readonly Consumer _consumer = new();

    [GlobalSetup]
    public void Setup()
    {
        new LambdaFixture().SetUpContext();
    }

    [Benchmark]
    public void Reflection()
    {
        var testFixture = new ReflectionFixture();
        var context = testFixture.CreateContext();
        context.Movies.AsEnumerable().Consume(_consumer);
    }
    
    [Benchmark(Baseline = true)]
    public void Lambda()
    {
        var testFixture = new LambdaFixture();
        var context = testFixture.CreateContext();
        context.Movies.AsEnumerable().Consume(_consumer);
    }
}