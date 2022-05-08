using BenchmarkDotNet.Attributes;

namespace PersistentDb.Benchmarks;

public class Benchmarks
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
        var testFixture = new ReflectionFixture();
        var context = testFixture.CreateContext();
        Count += context.Movies.Count();
    }

    [Benchmark]
    public void ReflectionNonStatic()
    {
        var testFixture = new ReflectionFixtureNonStatic();
        var context = testFixture.CreateContext();
        Count += context.Movies.Count();
    }

    [Benchmark(Baseline = true)]
    public void Lambda()
    {
        var testFixture = new LambdaFixture();
        var context = testFixture.CreateContext();
        Count += context.Movies.Count();
    }
}