using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Handlers
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            var assembly = typeof(DependenciesInjection).Assembly;


            services.AddValidatorsFromAssembly(assembly);

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes
                    .AssignableToAny(
                        typeof(ICommandHandler<,>),
                        typeof(ICommandHandler<>)
                    )
                )
                .AsSelfWithInterfaces()
            .WithScopedLifetime());


            return services;
        }
    }
}
