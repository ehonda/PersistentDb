using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PersistentDb.TestUtilities.Internal;

namespace PersistentDb.TestUtilities;

[PublicAPI]
// ReSharper disable once InconsistentNaming
public class TestFixtureWithDbContext_ReflectedConstructor<TDerivedContext> where TDerivedContext : DbContext
{
    private static ReflectedDerivedContextConstructor<TDerivedContext> _constructor = new();

    protected readonly Func<TDerivedContext> CreateContext;

    protected TDerivedContext Context = null!;

    public TestFixtureWithDbContext_ReflectedConstructor() => CreateContext = _constructor.Invoke;

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