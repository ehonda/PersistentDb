using System;
using PersistentDb.TestUtilities;

namespace PersistentDb.Tests;

public class MovieFixture : TestFixtureWithDbContext<MovieDbContext>
{
    public new Func<MovieDbContext> CreateContext => base.CreateContext;
}