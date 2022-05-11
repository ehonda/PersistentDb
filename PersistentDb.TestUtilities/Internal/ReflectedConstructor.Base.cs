using System.Reflection;
using JetBrains.Annotations;

namespace PersistentDb.TestUtilities.Internal;

internal partial class ReflectedConstructor<TTypeToConstruct>
{
    private readonly ConstructorInfo? _constructor;

    private readonly IReadOnlyCollection<Type> _constructorArgumentTypes;

    private readonly IReadOnlyCollection<Func<object?>> _constructorArgumentCreators;

    protected ReflectedConstructor(params (Func<object?> CreateConstructorArgument, Type ConstructorArgumentType)[]
        constructorArgumentData)
    {
        _constructorArgumentTypes = constructorArgumentData
            .Select(data => data.ConstructorArgumentType)
            .ToArray();

        _constructorArgumentCreators = constructorArgumentData
            .Select(data => data.CreateConstructorArgument)
            .ToArray();
        
        _constructor = typeof(TTypeToConstruct).GetConstructor(_constructorArgumentTypes.ToArray());
    }

    public TTypeToConstruct Invoke()
    {
        if (_constructor is null)
            throw new InvalidOperationException(NoSuitableConstructorMessage());

        return (TTypeToConstruct) _constructor.Invoke(_constructorArgumentCreators
            .Select(creator => creator()).ToArray());
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