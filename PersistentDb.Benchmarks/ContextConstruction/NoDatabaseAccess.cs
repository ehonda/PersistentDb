using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PersistentDb.Benchmarks.ContextConstruction;

[BenchmarkCategory(nameof(NoDatabaseAccess))]
public class NoDatabaseAccess : BenchmarksBase
{
    private static DbContextOptions<MovieDbContext> DefaultOptions { get; } =
        OptionsFromConnectionString(null);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(DirectConstructionCategory)]
    public void DirectConstruction()
    {
        using var context = new MovieDbContext(DefaultOptions);
        
        Consumer += context.GetHashCode();
    }

    [Benchmark]
    [BenchmarkCategory(InvokeReflectedConstructorCategory)]
    public void InvokeReflectedConstructor()
    {
        using var context = (MovieDbContext) MovieDbContextConstructor.Invoke(new object?[] { DefaultOptions });
        
        Consumer += context.GetHashCode();
    }

    [Benchmark]
    [BenchmarkCategory(FullReflectionCategory)]
    public void FullReflection()
    {
        var constructor = typeof(MovieDbContext).GetConstructor(new[] { typeof(DbContextOptions<MovieDbContext>) });
        using var context = (MovieDbContext) constructor!.Invoke(new object?[] { DefaultOptions });
        
        Consumer += context.GetHashCode();
    }
}