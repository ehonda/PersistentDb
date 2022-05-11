using BenchmarkDotNet.Attributes;

namespace PersistentDb.Benchmarks.TestFixtures;

[BenchmarkCategory(nameof(SetUpAndTearDown), "TestFixtures")]
public class SetUpAndTearDown
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory(nameof(ReflectionInClass))]
    public void ReflectionInClass()
    {
        var testFixture = new ReflectionFixture();
        testFixture.SetUpContext();
        testFixture.TearDownContext();
    }

    [Benchmark]
    [BenchmarkCategory(nameof(ReflectedConstructor))]
    public void ReflectedConstructor()
    {
        var testFixture = new ReflectionFixture_ReflectedConstructor();
        testFixture.SetUpContext();
        testFixture.TearDownContext();
    }
}