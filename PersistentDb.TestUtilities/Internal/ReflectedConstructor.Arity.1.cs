namespace PersistentDb.TestUtilities.Internal;

internal class ReflectedConstructor<TArgument0, TTypeToConstruct>
    : ReflectedConstructor<TTypeToConstruct>
{
    public ReflectedConstructor(Func<TArgument0> createConstructorArgument0)
        : base((() => createConstructorArgument0(), typeof(TArgument0)))
    {
    }
}