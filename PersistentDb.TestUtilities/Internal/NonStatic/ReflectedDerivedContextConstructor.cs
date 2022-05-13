using Microsoft.EntityFrameworkCore;

namespace PersistentDb.TestUtilities.Internal.NonStatic;

internal class ReflectedDerivedContextConstructor<TDerivedContext>
    : ReflectedConstructor<DbContextOptions<TDerivedContext>, TDerivedContext>
    where TDerivedContext : DbContext
{
    private static DbContextOptions<TDerivedContext> DbContextOptions =>
        new DbContextOptionsBuilder<TDerivedContext>()
            .UseSqlite("DataSource=test.db")
            .Options;

    public ReflectedDerivedContextConstructor() : base(() => DbContextOptions)
    {
    }
}