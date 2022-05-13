namespace PersistentDb.TestUtilities.Internal.NonStatic;

internal partial class ReflectedConstructor<TTypeToConstruct>
{
    public ReflectedConstructor()
        : this(Array.Empty<(Func<object?>, Type)>())
    {
    }
}