using FluentAssertions;
using NUnit.Framework;
using PersistentDb.TestUtilities;

namespace PersistentDb.Tests;

[TestFixture]
public class MovieTests : TestFixtureWithDbContext<MovieDbContext>
{
    [Test]
    public void Insert_Movie()
    {
        using (var dbContext = CreateContext())
        {
            dbContext.Movies.Add(new("Superhero movie"));
            dbContext.SaveChanges();
        }

        using (var dbContext = CreateContext())
        {
            dbContext.Movies.Should().ContainSingle().Which.Title.Should().Be("Superhero movie");
        }
    }
}