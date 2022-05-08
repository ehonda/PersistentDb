using System.Reflection;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PersistentDb.Benchmarks;

public class BenchmarksRaw
{
    public static int Count { get; private set; }

    private static DbContextOptions<MovieDbContext> InMemoryOptions { get; } =
        OptionsFromConnectionString("DataSource=:memory:");

    private static DbContextOptions<MovieDbContext> FileBasedOptions { get; } =
        OptionsFromConnectionString("DataSource=test.db");
    
    private static DbContextOptions<MovieDbContext> EmptyOptions { get; } =
        new DbContextOptionsBuilder<MovieDbContext>().Options;

    private static ConstructorInfo MovieDbContextConstructor { get; }
        = typeof(MovieDbContext).GetConstructor(new[] { typeof(DbContextOptions<MovieDbContext>) })!;

    private static DbContextOptions<MovieDbContext> OptionsFromConnectionString(string connectionString) =>
        new DbContextOptionsBuilder<MovieDbContext>()
            .UseSqlite(connectionString)
            .Options;

    [GlobalSetup]
    public void Setup()
    {
        Count = 0;
        using var context = new MovieDbContext(FileBasedOptions);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    [Benchmark]
    public void InvokeConstructorInMemory()
    {
        using var context = (MovieDbContext) MovieDbContextConstructor.Invoke(new object?[] { InMemoryOptions });

        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        Count += context.Movies.Count();
    }

    [Benchmark]
    public void InvokeConstructorFileBased()
    {
        using var context = (MovieDbContext) MovieDbContextConstructor.Invoke(new object?[] { FileBasedOptions });

        Count += context.Movies.Count();
    }
    
    [Benchmark]
    public void InvokeConstructorNoDatabaseAccess()
    {
        using var context = (MovieDbContext) MovieDbContextConstructor.Invoke(new object?[] { EmptyOptions });

        Count += context.ContextId.Lease;
    }

    [Benchmark]
    public void ReflectionInMemory()
    {
        var derivedContextType = typeof(MovieDbContext);
        var dbContextOptionsType = typeof(DbContextOptions<MovieDbContext>);

        var derivedContextConstructor = derivedContextType.GetConstructor(new[] { dbContextOptionsType });
        using var context = (MovieDbContext) derivedContextConstructor!.Invoke(new object?[] { InMemoryOptions });

        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        Count += context.Movies.Count();
    }

    [Benchmark]
    public void ReflectionFileBased()
    {
        var derivedContextType = typeof(MovieDbContext);
        var dbContextOptionsType = typeof(DbContextOptions<MovieDbContext>);

        var derivedContextConstructor = derivedContextType.GetConstructor(new[] { dbContextOptionsType });
        using var context = (MovieDbContext) derivedContextConstructor!.Invoke(new object?[] { FileBasedOptions });

        Count += context.Movies.Count();
    }
    
    [Benchmark]
    public void ReflectionNoDatabaseAccess()
    {
        var derivedContextType = typeof(MovieDbContext);
        var dbContextOptionsType = typeof(DbContextOptions<MovieDbContext>);

        var derivedContextConstructor = derivedContextType.GetConstructor(new[] { dbContextOptionsType });
        using var context = (MovieDbContext) derivedContextConstructor!.Invoke(new object?[] { EmptyOptions });

        Count += context.ContextId.Lease;
    }

    [Benchmark]
    public void DirectConstructionInMemory()
    {
        using var context = new MovieDbContext(InMemoryOptions);

        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        Count += context.Movies.Count();
    }

    [Benchmark]
    public void DirectConstructionFileBased()
    {
        using var context = new MovieDbContext(FileBasedOptions);

        Count += context.Movies.Count();
    }
    
    [Benchmark(Baseline = true)]
    public void DirectConstructionNoDatabaseAccess()
    {
        using var context = new MovieDbContext(EmptyOptions);

        Count += context.ContextId.Lease;
    }
}