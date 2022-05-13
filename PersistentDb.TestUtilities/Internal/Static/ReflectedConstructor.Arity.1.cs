namespace PersistentDb.TestUtilities.Internal.Static;

public static class ReflectedConstructor<TArgument0, TTypeToConstruct>
{
    private static readonly ReflectedConstructorWrapper<TTypeToConstruct> Constructor
        = new(typeof(TArgument0));

    public static TTypeToConstruct Invoke(TArgument0 arg0) => Constructor.Invoke(arg0);
}