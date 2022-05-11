namespace PersistentDb.TestUtilities.Internal;

internal partial class ReflectedConstructor<TTypeToConstruct>
{
    public ReflectedConstructor()
        : this(Array.Empty<(Func<object?>, Type)>())
    {
    }
}