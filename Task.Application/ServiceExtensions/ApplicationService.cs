using Clinic.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using PasySkyTask.Application.Contracts;
using System.Reflection;

namespace PaySkyTask.Application.ExtensionMethods;

public static class ApplicationService
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        var serviceTypes = Assembly.GetExecutingAssembly().ExportedTypes
                .Where(t => typeof(ILifeTime).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

        
        foreach (var serviceType in serviceTypes)
        {
            var interfaceType = serviceType.GetInterface("I" + serviceType.Name);
            var result = serviceType switch
            {
                _ when typeof(IScopedService).IsAssignableFrom(serviceType) => services.AddScoped(interfaceType, serviceType),
                _ when typeof(ITransientService).IsAssignableFrom(serviceType) => services.AddTransient(interfaceType, serviceType),
                _ when typeof(ISingletonService).IsAssignableFrom(serviceType) => services.AddSingleton(interfaceType, serviceType)
            };
        }
        return services;
    }
}
