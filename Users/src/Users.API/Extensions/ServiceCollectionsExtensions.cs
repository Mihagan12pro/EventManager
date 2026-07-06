using Shared.Infrastracture.Kafka;
using Shared.Objects.Classes.Options;
using Users.API.Contracts;

namespace Users.API.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddKafkaProducers(this IServiceCollection services)
        {
            KafkaOptions kafkaOptions = new KafkaOptions();

            services.AddProducer<JwtTokenContract>(kafkaOptions.FirstProducer("Jwt"));

            return services;
        }
    }
}
