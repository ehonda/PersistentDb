using PersistentDb.TestUtilities;

namespace PersistentDb.Benchmarks;

public class ReflectionFixtureNonStatic : TestFixtureWithDbContextNonStatic<MovieDbContext>
{
    public new Func<MovieDbContext> CreateContext => base.CreateContext;
}