using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace PersistentDb.Tests;

public static class Foo
{
    public static string Bar<T>(T t) => "Don't care";
}

public class ParameterizedTests
{
    private static class ValueSources
    {
        public static IEnumerable<int> Integers => new[] { 0, 1, 2 };

        public static IEnumerable<char> Characters => new[] { 'a', 'b', 'c' };
    }

    [Test]
    public void FooBar_does_not_care<T>(
        [ValueSource(typeof(ValueSources), nameof(ValueSources.Integers))]
        [ValueSource(typeof(ValueSources), nameof(ValueSources.Characters))]
        T input)
    {
        Foo.Bar(input).Should().Be("Don't care");
    }
}