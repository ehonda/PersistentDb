using BenchmarkDotNet.Attributes;

namespace PersistentDb.Benchmarks.TestFixtures;

[BenchmarkCategory(nameof(FixtureConstruction), "TestFixtures")]
public class FixtureConstruction
{
    public static int Count { get; private set; }

    [GlobalSetup]
    public void Setup()
    {
        Count = 0;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(nameof(ReflectionInClass))]
    public void ReflectionInClass()
    {
        var testFixture = new ReflectionFixture();
        Count += testFixture.GetHashCode();
    }

    [Benchmark]
    [BenchmarkCategory(nameof(ReflectedConstructor))]
    public void ReflectedConstructor()
    {
        var testFixture = new ReflectionFixture_ReflectedConstructor();
        Count += testFixture.GetHashCode();
    }
}