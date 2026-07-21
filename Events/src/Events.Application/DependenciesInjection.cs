using Events.Application.Singleton.Cache.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Objects.Interfaces;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Events.Unit")]
namespace Events.Application
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddCacheKeys(configuration);
            services.AddHandlers();

            return services;
        }

        private static IServiceCollection AddCacheKeys(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.Configure<CacheKeysOptions>(
                configuration.GetRequiredSection(nameof(CacheKeysOptions)));

            return services;
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
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
