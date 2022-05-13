namespace PersistentDb.TestUtilities.Internal.Static;

public static class ReflectedConstructor<TArgument0, TArgument1, TTypeToConstruct>
{
    private static readonly ReflectedConstructorWrapper<TTypeToConstruct> Constructor
        = new(typeof(TArgument0), typeof(TArgument1));

    public static TTypeToConstruct Invoke(TArgument0 arg0, TArgument1 arg1) => Constructor.Invoke(arg0, arg1);
}