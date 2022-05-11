using PersistentDb.TestUtilities;

namespace PersistentDb.Benchmarks;

public class ReflectionFixture_ReflectedConstructor : TestFixtureWithDbContext_ReflectedConstructor<MovieDbContext>
{
    public new Func<MovieDbContext> CreateContext => base.CreateContext;
}