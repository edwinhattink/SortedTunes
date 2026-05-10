using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SortedTunes.Mediator;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, Action<MediatorServiceConfiguration> configure)
    {
        var config = new MediatorServiceConfiguration();
        configure(config);

        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<ISender>(sp => sp.GetRequiredService<IMediator>());
        services.AddTransient<IPublisher>(sp => sp.GetRequiredService<IMediator>());

        foreach (var assembly in config.AssembliesToRegister)
        {
            RegisterHandlersFromAssembly(services, assembly);
        }

        foreach (var (serviceType, implementationType) in config.Behaviors)
        {
            services.AddTransient(serviceType, implementationType);
        }

        return services;
    }

    private static void RegisterHandlersFromAssembly(IServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => !t.IsGenericTypeDefinition && !t.IsAbstract && !t.IsInterface);

        foreach (var type in types)
        {
            var handlerInterfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && IsHandlerInterface(i.GetGenericTypeDefinition()));

            foreach (var @interface in handlerInterfaces)
            {
                services.AddTransient(@interface, type);
            }
        }
    }

    private static bool IsHandlerInterface(Type genericDefinition) =>
        genericDefinition == typeof(IRequestHandler<,>) ||
        genericDefinition == typeof(IRequestHandler<>) ||
        genericDefinition == typeof(INotificationHandler<>);
}
