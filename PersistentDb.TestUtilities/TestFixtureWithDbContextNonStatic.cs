using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace PersistentDb.TestUtilities;

[PublicAPI]
public class TestFixtureWithDbContextNonStatic<TDerivedContext> where TDerivedContext : DbContext
{
    protected readonly Func<TDerivedContext> CreateContext;

    public TestFixtureWithDbContextNonStatic()
    {
        var derivedContextConstructor = DerivedContextType.GetConstructor(new[] { DbContextOptionsType });

        if (derivedContextConstructor is null)
            throw new InvalidOperationException(
                $"Could not find a suitable constructor for {DerivedContextType}. Please provide a public " +
                $"constructor taking a single parameter of type {DbContextOptionsType}");

        CreateContext = () => (TDerivedContext) derivedContextConstructor
            .Invoke(new object?[] { DbContextOptions });
    }

    private static Type DerivedContextType => typeof(TDerivedContext);

    private static Type DbContextOptionsType => typeof(DbContextOptions<TDerivedContext>);

    private static DbContextOptions<TDerivedContext> DbContextOptions =>
        new DbContextOptionsBuilder<TDerivedContext>()
            .UseSqlite("DataSource=test.db")
            .Options;
}