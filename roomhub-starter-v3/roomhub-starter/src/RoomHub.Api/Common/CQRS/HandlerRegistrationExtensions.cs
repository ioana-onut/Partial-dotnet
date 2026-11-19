using System.Reflection;

namespace RoomHub.Api.Common.CQRS;

public static class HandlerRegistrationExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false });

        foreach (var implementation in handlerTypes)
        {
            foreach (var service in implementation.GetInterfaces())
            {
                if (!service.IsGenericType)
                {
                    continue;
                }

                var definition = service.GetGenericTypeDefinition();
                if (definition == typeof(ICommandHandler<,>) || definition == typeof(IQueryHandler<,>))
                {
                    services.AddTransient(service, implementation);
                }
            }
        }

        return services;
    }
}
