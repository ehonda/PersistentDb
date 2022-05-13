using Microsoft.EntityFrameworkCore;

namespace PersistentDb.TestUtilities.Internal.Static;

internal static class ReflectedDerivedContextConstructor<TDerivedContext>
    where TDerivedContext : DbContext
{
    private static DbContextOptions<TDerivedContext> DbContextOptions =>
        new DbContextOptionsBuilder<TDerivedContext>()
            .UseSqlite("DataSource=test.db")
            .Options;

    public static TDerivedContext Invoke()
        => ReflectedConstructor<DbContextOptions<TDerivedContext>, TDerivedContext>.Invoke(DbContextOptions);
}