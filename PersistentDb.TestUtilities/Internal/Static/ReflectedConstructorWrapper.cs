using System.Reflection;
using JetBrains.Annotations;

namespace PersistentDb.TestUtilities.Internal.Static;

internal class ReflectedConstructorWrapper<TTypeToConstruct>
{
    private readonly ConstructorInfo? _constructor;

    private readonly IReadOnlyCollection<Type> _constructorArgumentTypes;

    public ReflectedConstructorWrapper(params Type[] constructorArgumentTypes)
    {
        _constructorArgumentTypes = constructorArgumentTypes
            .ToArray();

        _constructor = typeof(TTypeToConstruct).GetConstructor(_constructorArgumentTypes.ToArray());
    }

    public TTypeToConstruct Invoke(params object?[] constructorArguments)
    {
        if (_constructor is null)
            throw new InvalidOperationException(NoSuitableConstructorMessage());

        return (TTypeToConstruct) _constructor.Invoke(constructorArguments.ToArray());
    }

    [Pure]
    private string NoSuitableConstructorMessage()
    {
        var constructorArgumentsString = string.Join(", ",
            _constructorArgumentTypes.Select(type => type.ToString()));

        var typeToConstruct = typeof(TTypeToConstruct);

        return $@"Could not find a suitable constructor for {typeToConstruct}.
Please provide a constructor with the following signature:
  public {typeToConstruct}({constructorArgumentsString})";
    }
}