using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastracture.Kafka;
using Shared.Objects.Classes.Options;
using Shared.Objects.Interfaces;

namespace Events.Application
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

        public static IServiceCollection AddKafkaInfrastracture(this IServiceCollection services)
        {
            KafkaOptions kafkaOptions = new KafkaOptions();

            //services.AddProducer<JwtTokenContract>(kafkaOptions.FirstProducer("Jwt"));

            return services;
        }
    }
}
