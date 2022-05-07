using PersistentDb.TestUtilities;

namespace PersistentDb.Benchmarks;

public class LambdaFixture : TestFixtureWithDbContext<MovieDbContext>
{
    public LambdaFixture() : base(options => new(options))
    {
    }

    public new Func<MovieDbContext> CreateContext => base.CreateContext;
}