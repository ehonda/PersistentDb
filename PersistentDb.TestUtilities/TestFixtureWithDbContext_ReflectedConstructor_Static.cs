using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PersistentDb.TestUtilities.Internal.Static;

namespace PersistentDb.TestUtilities;

[PublicAPI]
// ReSharper disable once InconsistentNaming
public class TestFixtureWithDbContext_ReflectedConstructor_Static<TDerivedContext> where TDerivedContext : DbContext
{
    protected readonly Func<TDerivedContext> CreateContext = ReflectedDerivedContextConstructor<TDerivedContext>.Invoke;

    protected TDerivedContext Context = null!;

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