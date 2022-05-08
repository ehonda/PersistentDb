using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PersistentDb.Benchmarks.ContextConstruction;

[BenchmarkCategory(nameof(FileBasedDatabase))]
public class FileBasedDatabase : BenchmarksBase
{
    private static DbContextOptions<MovieDbContext> FileBasedDatabaseOptions { get; } =
        OptionsFromConnectionString("DataSource=test.db");

    [GlobalSetup]
    public static void Setup()
    {
        using var context = new MovieDbContext(FileBasedDatabaseOptions);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(DirectConstructionCategory)]
    public void DirectConstruction()
    {
        using var context = new MovieDbContext(FileBasedDatabaseOptions);
        
        Consumer += context.Movies.Count();
    }

    [Benchmark]
    [BenchmarkCategory(InvokeReflectedConstructorCategory)]
    public void InvokeReflectedConstructor()
    {
        using var context = (MovieDbContext) MovieDbContextConstructor.Invoke(new object?[] { FileBasedDatabaseOptions });
        
        Consumer += context.Movies.Count();
    }

    [Benchmark]
    [BenchmarkCategory(FullReflectionCategory)]
    public void FullReflection()
    {
        var constructor = typeof(MovieDbContext).GetConstructor(new[] { typeof(DbContextOptions<MovieDbContext>) });
        using var context = (MovieDbContext) constructor!.Invoke(new object?[] { FileBasedDatabaseOptions });
        
        Consumer += context.Movies.Count();
    }
}