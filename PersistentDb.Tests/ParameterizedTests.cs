using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace PersistentDb.Tests;

public static class Foo
{
    public static string Bar<T>(T t) => "Don't care";
}

public class GenFoo<T>
{
    public string Bar() => "Don't care";
}

public class StringFoo : GenFoo<string> { }

public class IntFoo : GenFoo<int> { }

[Explicit]
public class ParameterizedTests
{
    private static class ValueSources
    {
        public static IEnumerable<int> Integers => new[] { 0, 1, 2 };

        public static IEnumerable<char> Characters => new[] { 'a', 'b', 'c' };

        public static IEnumerable<StringFoo> StringFoosDerived => new StringFoo[] { new() };
        
        public static IEnumerable<IntFoo> IntFoosDerived => new IntFoo[] { new() };
        
        public static IEnumerable<GenFoo<string>> StringFoosBase => new StringFoo[] { new() };
        
        public static IEnumerable<GenFoo<int>> IntFoosBase => new IntFoo[] { new() };
        
        public static IEnumerable<(GenFoo<string> Foo, string Type)> StringFoosBaseWithType
            => new (GenFoo<string> Foo, string Type)[] { (new(), string.Empty) };
        
        public static IEnumerable<(GenFoo<int> Foo, int Type)> IntFoosBaseWithType
            => new (GenFoo<int> Foo, int Type)[] { (new(), 0) };
        
        public static IEnumerable<(object Foo, string Type)> StringFoosObjectWithType
            => new (object Foo, string Type)[] { (new GenFoo<string>(), string.Empty) };
        
        public static IEnumerable<(object Foo, int Type)> IntFoosObjectWithType
            => new (object Foo, int Type)[] { (new GenFoo<int>(), 0) };
        
        public static IEnumerable<(object Foo, GenFoo<string> Type)> StringFoosObjectWithFullType
            => new (object Foo, GenFoo<string> Type)[] { (new GenFoo<string>(), default!) };
    }

    [Test]
    public void FooBar_does_not_care<T>(
        [ValueSource(typeof(ValueSources), nameof(ValueSources.Integers))]
        [ValueSource(typeof(ValueSources), nameof(ValueSources.Characters))]
        T input)
    {
        Foo.Bar(input).Should().Be("Don't care");
    }
    
    // Unable to determine type arguments for method
    [Test]
    public void Derived_Gen_Foo_Sources<TFoo, T>(
        [ValueSource(typeof(ValueSources), nameof(ValueSources.StringFoosDerived))]
        [ValueSource(typeof(ValueSources), nameof(ValueSources.IntFoosDerived))]
        TFoo foo)
        where TFoo : GenFoo<T>
    {
        foo.Bar().Should().Be("Don't care");
    }
    
    // Unable to determine type arguments for method
    [Test]
    public void Base_Gen_Foo_Sources<T>(
        [ValueSource(typeof(ValueSources), nameof(ValueSources.StringFoosBase))]
        [ValueSource(typeof(ValueSources), nameof(ValueSources.IntFoosBase))]
        GenFoo<T> foo)
    {
        foo.Bar().Should().Be("Don't care");
    }
    
    // Unable to determine type arguments for method
    [Test]
    public void Base_Gen_Foo_Sources_With_Type<T>(
        [ValueSource(typeof(ValueSources), nameof(ValueSources.StringFoosBase))]
        // [ValueSource(typeof(ValueSources), nameof(ValueSources.IntFoosBase))]
        (GenFoo<T> Foo, T) data)
    {
        data.Foo.Bar().Should().Be("Don't care");
    }
    
    [Test]
    public void Object_With_Type<T>(
        [ValueSource(typeof(ValueSources), nameof(ValueSources.StringFoosObjectWithType))]
        [ValueSource(typeof(ValueSources), nameof(ValueSources.IntFoosObjectWithType))]
        (object Foo, T) data)
    {
        ((GenFoo<T>)data.Foo).Bar().Should().Be("Don't care");
    }
    
    // Unable to determine type arguments for method
    [Test]
    public void Object_With_Full_Type<T>(
        [ValueSource(typeof(ValueSources), nameof(ValueSources.StringFoosObjectWithType))]
        [ValueSource(typeof(ValueSources), nameof(ValueSources.IntFoosObjectWithType))]
        (object Foo, GenFoo<T>) data)
    {
        ((GenFoo<T>)data.Foo).Bar().Should().Be("Don't care");
    }
}