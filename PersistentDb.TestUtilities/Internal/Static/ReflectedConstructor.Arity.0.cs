namespace PersistentDb.TestUtilities.Internal.Static;

public static class ReflectedConstructor<TTypeToConstruct>
{
    private static readonly ReflectedConstructorWrapper<TTypeToConstruct> Constructor = new();

    public static TTypeToConstruct Invoke() => Constructor.Invoke();
}