namespace PersistentDb.TestUtilities.Internal.NonStatic;

internal class ReflectedConstructor<TArgument0, TTypeToConstruct>
    : ReflectedConstructor<TTypeToConstruct>
{
    public ReflectedConstructor(Func<TArgument0> createConstructorArgument0)
        : base((() => createConstructorArgument0(), typeof(TArgument0)))
    {
    }
}