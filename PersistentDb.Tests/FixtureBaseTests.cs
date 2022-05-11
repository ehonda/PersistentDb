using System.IO;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using PersistentDb.TestUtilities;

namespace PersistentDb.Tests;

public class FixtureBaseTests
{
    [Test]
    public void SetUp_creates_file()
    {
        var fixture = new TestFixtureWithDbContext<MovieDbContext>();
        fixture.SetUpContext();
        File.Exists("test.db").Should().BeTrue();
    }
    
    [Test]
    public void TearDown_deletes_file()
    {
        var fixture = new TestFixtureWithDbContext<MovieDbContext>();
        fixture.SetUpContext();
        fixture.TearDownContext();
        File.Exists("test.db").Should().BeFalse();
    }
    
    [Test]
    public void SetUp_creates_file_ReflectedConstructor()
    {
        var fixture = new TestFixtureWithDbContext_ReflectedConstructor<MovieDbContext>();
        fixture.SetUpContext();
        File.Exists("test.db").Should().BeTrue();
    }
    
    [Test]
    public void TearDown_deletes_file_ReflectedConstructor()
    {
        var fixture = new TestFixtureWithDbContext_ReflectedConstructor<MovieDbContext>();
        fixture.SetUpContext();
        fixture.TearDownContext();
        File.Exists("test.db").Should().BeFalse();
    }

    [Test]
    public void Db_File_is_locked()
    {
        using var fileStream = new FileStream("test.db", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

        var fixture = new MovieFixture();
        
        var creation = () =>
        {
            using var context = fixture.CreateContext();
            return context.Database.EnsureCreated();
        };

        creation.Should().Throw<SqliteException>();
    }
}