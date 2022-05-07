using PersistentDb.TestUtilities;

namespace PersistentDb.Benchmarks;

public class ReflectionFixture : TestFixtureWithDbContext<MovieDbContext>
{
    public new Func<MovieDbContext> CreateContext => base.CreateContext;
}