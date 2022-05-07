using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace PersistentDb.TestUtilities;

[PublicAPI]
public class TestFixtureWithDbContext<TDerivedContext> where TDerivedContext : DbContext
{
    protected readonly Func<TDerivedContext> CreateContext;

    protected TDerivedContext Context = null!;

    public TestFixtureWithDbContext()
    {
        if (DerivedContextConstructor is null)
            throw new InvalidOperationException(
                $"Could not find a suitable constructor for {DerivedContextType}. Please provide a public " +
                $"constructor taking a single parameter of type {DbContextOptionsType}");

        CreateContext = () => (TDerivedContext) DerivedContextConstructor
            .Invoke(new object?[] { CreateDbContextOptions() });
    }

    public TestFixtureWithDbContext(Func<DbContextOptions<TDerivedContext>, TDerivedContext> derivedContextConstructor)
    {
        CreateContext = () => derivedContextConstructor(CreateDbContextOptions());
    }

    private static Type DerivedContextType => typeof(TDerivedContext);

    private static Type DbContextOptionsType => typeof(DbContextOptions<TDerivedContext>);

    // ReSharper disable once StaticMemberInGenericType
    private static ConstructorInfo? DerivedContextConstructor { get; }
        = DerivedContextType.GetConstructor(new[] { DbContextOptionsType });

    private static DbContextOptions<TDerivedContext> CreateDbContextOptions() =>
        new DbContextOptionsBuilder<TDerivedContext>()
            .UseSqlite("DataSource=test.db")
            .Options;

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
}