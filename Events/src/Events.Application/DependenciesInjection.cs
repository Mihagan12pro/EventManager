using Events.Application.Singleton.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Objects.Classes.Options;
using Shared.Objects.Interfaces;

namespace Events.Application
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddSingleton(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddSingleton<CacheKeysService>();

            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            var assembly = typeof(DependenciesInjection).Assembly;

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes
                    .AssignableToAny(
                        typeof(ICommandHandler<,>),
                        typeof(ICommandHandler<>)
                    ),
                    publicOnly: false
                )
                .AsSelfWithInterfaces()
            .WithScopedLifetime());

            return services;
        }
    }
}
