using BenchmarkDotNet.Attributes;

namespace PersistentDb.Benchmarks.TestFixtures;

[BenchmarkCategory(nameof(ContextUsage), "TestFixtures")]
public class ContextUsage
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
        testFixture.SetUpContext();
        
        var context = testFixture.CreateContext();
        Count += context.Movies.Count();
        
        testFixture.TearDownContext();
    }

    [Benchmark]
    [BenchmarkCategory(nameof(ReflectedConstructor))]
    public void ReflectedConstructor()
    {
        var testFixture = new ReflectionFixture_ReflectedConstructor();
        testFixture.SetUpContext();
        
        var context = testFixture.CreateContext();
        Count += context.Movies.Count();
        
        testFixture.TearDownContext();
    }
}