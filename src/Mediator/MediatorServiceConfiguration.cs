using System.Reflection;

namespace SortedTunes.Mediator;

public class MediatorServiceConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; } = [];
    internal List<(Type ServiceType, Type ImplementationType)> Behaviors { get; } = [];

    public MediatorServiceConfiguration RegisterServicesFromAssembly(Assembly assembly)
    {
        AssembliesToRegister.Add(assembly);
        return this;
    }

    public MediatorServiceConfiguration AddBehavior(Type serviceType, Type implementationType)
    {
        Behaviors.Add((serviceType, implementationType));
        return this;
    }
}
