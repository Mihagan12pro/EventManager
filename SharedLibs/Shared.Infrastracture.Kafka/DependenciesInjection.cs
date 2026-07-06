using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastracture.Kafka.Producers;

namespace Shared.Infrastracture.Kafka
{
    public static class DependenciesInjection
    {
        public static void AddProducer<TMessage>(this IServiceCollection services,
            IConfigurationSection configurationSection)
        {
            services.Configure<KafkaProducerSettings>(configurationSection);
            services.AddSingleton<IKafkaProducer<TMessage>, KafkaProducer<TMessage>>();
        }
    }
}
