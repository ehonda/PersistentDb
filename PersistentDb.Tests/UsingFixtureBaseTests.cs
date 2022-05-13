using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PersistentDb.TestUtilities;

namespace PersistentDb.Tests;

public class UsingFixtureBaseTests : TestFixtureWithDbContext_ReflectedConstructor<MovieDbContext>
{
    [Test]
    public void DbContext_parallel()
    {
        Context.Movies.Add(new("Action Movie"));
        Context.SaveChanges();

        using (var context = CreateContext())
        {
            context.Movies.Single().Title = "Documentary";
            context.SaveChanges();
        }
        
        using (var context = CreateContext())
        {
            context.Movies.Single().Title.Should().Be("Documentary");
        }

        var documentary = Context.Movies.Single(movie => movie.Title == "Documentary");
        documentary.Title.Should().Be("Documentary");
        var actionMovie = Context.Movies.Single();

        Context.Movies.Should().HaveCount(1);
        Context.Movies.Where(movie => movie.Title == "Documentary").Should().HaveCount(1);
        Context.Movies.First().Title.Should().Be("Documentary");
        Context.Movies.Single().Title.Should().Be("Documentary");
    }
}