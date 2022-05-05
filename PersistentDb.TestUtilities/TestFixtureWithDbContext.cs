using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace PersistentDb.TestUtilities;

[PublicAPI]
public class TestFixtureWithDbContext<TDerivedContext> where TDerivedContext : DbContext
{
    protected readonly Func<TDerivedContext> CreateContext;

    protected TDerivedContext Context = null!;

    [SetUp]
    public void SetUpContext()
    {
        Context = CreateContext();
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDownContext()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }

    public TestFixtureWithDbContext()
    {
        var derivedContextType = typeof(TDerivedContext);
        var dbContextOptionsType = typeof(DbContextOptions<TDerivedContext>);
        
        var constructor = derivedContextType
            .GetConstructor(new[] { dbContextOptionsType });

        if (constructor is null)
            throw new InvalidOperationException(
                $"Could not find a suitable constructor for {derivedContextType}. Please provide a public " +
                $"constructor taking a single parameter of type {dbContextOptionsType}");

        CreateContext = () =>
        {
            var options = new DbContextOptionsBuilder<TDerivedContext>()
                .UseSqlite("DataSource=test.db")
                .Options;

            return (constructor.Invoke(new object?[] { options }) as TDerivedContext)!;
        };
    }
}