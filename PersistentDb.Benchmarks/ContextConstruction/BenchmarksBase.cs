using System.Reflection;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace PersistentDb.Benchmarks.ContextConstruction;

public class BenchmarksBase
{
    protected const string DirectConstructionCategory = "DirectConstruction";

    protected const string InvokeReflectedConstructorCategory = "InvokeReflectedConstructor";

    protected const string FullReflectionCategory = "FullReflection";
    
    public static int Consumer { get; protected set; }

    protected static ConstructorInfo MovieDbContextConstructor { get; }
        = typeof(MovieDbContext).GetConstructor(new[] { typeof(DbContextOptions<MovieDbContext>) })!;

    protected static DbContextOptions<MovieDbContext> OptionsFromConnectionString(string? connectionString)
    {
        var builder = new DbContextOptionsBuilder<MovieDbContext>();

        if (connectionString is not null)
            builder.UseSqlite(connectionString);

        return builder.Options;
    }

    [GlobalSetup(Target = nameof(BenchmarksBase))]
    public static void BaseSetup()
    {
        Consumer = 0;
    }
}