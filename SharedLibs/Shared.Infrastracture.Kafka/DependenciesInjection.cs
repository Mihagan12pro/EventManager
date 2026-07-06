using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastracture.Kafka.Consumers;
using Shared.Infrastracture.Kafka.Producers;

namespace Shared.Infrastracture.Kafka
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddProducer<TMessage>(this IServiceCollection services,
            IConfigurationSection configurationSection)
        {
            services.Configure<KafkaProducerSettings>(configurationSection);
            services.AddSingleton<IKafkaProducer<TMessage>, KafkaProducer<TMessage>>();

            return services;
        }

        public static IServiceCollection AddConsumer<TMessage, TMessageHandler>(
            this IServiceCollection services, IConfigurationSection configurationSection) 
            where TMessageHandler : class, IMessageHandler<TMessage>
        {
            services.Configure<KafkaConsumerSettings>(configurationSection);
            services.AddSingleton<KafkaConsumer<TMessage>>();
            services.AddSingleton<IMessageHandler<TMessage>, TMessageHandler>();

            return services;
        }
    }
}
