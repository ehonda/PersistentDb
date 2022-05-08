using System.Reflection;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PersistentDb.Benchmarks;

public class BenchmarksNoDbAccess
{
    public static int Count { get; private set; }
    
    private static DbContextOptions<MovieDbContext> EmptyOptions { get; } =
        new DbContextOptionsBuilder<MovieDbContext>().Options;

    private static ConstructorInfo MovieDbContextConstructor { get; }
        = typeof(MovieDbContext).GetConstructor(new[] { typeof(DbContextOptions<MovieDbContext>) })!;

    [GlobalSetup]
    public void Setup()
    {
        Count = 0;
    }
    
    [Benchmark]
    public void InvokeConstructor()
    {
        using var context = (MovieDbContext) MovieDbContextConstructor.Invoke(new object?[] { EmptyOptions });

        Count += context.ContextId.Lease;
    }
    
    [Benchmark]
    public void Reflection()
    {
        var derivedContextType = typeof(MovieDbContext);
        var dbContextOptionsType = typeof(DbContextOptions<MovieDbContext>);

        var derivedContextConstructor = derivedContextType.GetConstructor(new[] { dbContextOptionsType });
        using var context = (MovieDbContext) derivedContextConstructor!.Invoke(new object?[] { EmptyOptions });

        Count += context.ContextId.Lease;
    }
    
    [Benchmark(Baseline = true)]
    public void DirectConstruction()
    {
        using var context = new MovieDbContext(EmptyOptions);

        Count += context.ContextId.Lease;
    }
}