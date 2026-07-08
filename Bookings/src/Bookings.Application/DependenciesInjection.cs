using Microsoft.Extensions.DependencyInjection;
using Shared.Objects.Interfaces;

namespace Bookings.Application
{
    public static class DependenciesInjection
    {
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
