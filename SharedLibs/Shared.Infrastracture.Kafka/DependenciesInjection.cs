using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastracture.Kafka.Consumers;
using Shared.Infrastracture.Kafka.Producers;
using Shared.Messaging;
using Shared.Objects.Classes.Options;

namespace Shared.Infrastracture.Kafka
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddProducer<TMessage>(
            this IServiceCollection services,
            string topic) where TMessage : IMessage
        {
            KafkaOptions options = new KafkaOptions();

            services.Configure<KafkaProducerSettings>(options.FirstProducer(topic));
            services.AddSingleton<IKafkaProducer<TMessage>, KafkaProducer<TMessage>>();

            return services;
        }

        public static IServiceCollection AddConsumer<TMessage, TMessageHandler>(
            this IServiceCollection services, string topic) 
            where TMessageHandler : class, IMessageHandler<TMessage>
            where TMessage : IMessage
        {
            KafkaOptions options = new KafkaOptions();

            services.Configure<KafkaConsumerSettings>(options.FirstConsumer(topic));
            services.AddHostedService<KafkaConsumer<TMessage>>();
            services.AddSingleton<IMessageHandler<TMessage>, TMessageHandler>();

            return services;
        }
    }
}
