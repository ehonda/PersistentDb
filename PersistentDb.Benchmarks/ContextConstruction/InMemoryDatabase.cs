using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PersistentDb.Benchmarks.ContextConstruction;

[BenchmarkCategory(nameof(InMemoryDatabase))]
public class InMemoryDatabase : BenchmarksBase
{
    private static DbContextOptions<MovieDbContext> InMemoryDatabaseOptions { get; } =
        OptionsFromConnectionString("DataSource=:memory:");

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(DirectConstructionCategory)]
    public void DirectConstruction()
    {
        using var context = new MovieDbContext(InMemoryDatabaseOptions);
        
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        Consumer += context.Movies.Count();
    }

    [Benchmark]
    [BenchmarkCategory(InvokeReflectedConstructorCategory)]
    public void InvokeReflectedConstructor()
    {
        using var context = (MovieDbContext) MovieDbContextConstructor.Invoke(new object?[] { InMemoryDatabaseOptions });
        
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        Consumer += context.Movies.Count();
    }

    [Benchmark]
    [BenchmarkCategory(FullReflectionCategory)]
    public void FullReflection()
    {
        var constructor = typeof(MovieDbContext).GetConstructor(new[] { typeof(DbContextOptions<MovieDbContext>) });
        using var context = (MovieDbContext) constructor!.Invoke(new object?[] { InMemoryDatabaseOptions });
        
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        Consumer += context.Movies.Count();
    }
}