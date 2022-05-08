using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.EntityFrameworkCore;

namespace PersistentDb.Benchmarks;

public class BenchmarksRaw
{
    public static int Count { get; private set; }
    
    private static DbContextOptions<MovieDbContext> DbContextOptions { get; } =
        new DbContextOptionsBuilder<MovieDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

    [GlobalSetup]
    public void Setup()
    {
        Count = 0;
    }

    [Benchmark]
    public void Reflection()
    {
        var derivedContextType = typeof(MovieDbContext);
        var dbContextOptionsType = typeof(DbContextOptions<MovieDbContext>);
        
        var derivedContextConstructor = derivedContextType.GetConstructor(new[] { dbContextOptionsType });
        using var context = (MovieDbContext) derivedContextConstructor!.Invoke(new object?[] { DbContextOptions });
        
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        Count += context.Movies.Count();
    }

    [Benchmark(Baseline = true)]
    public void DirectConstruction()
    {
        using var context = new MovieDbContext(DbContextOptions);
        
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        Count += context.Movies.Count();
    }
}