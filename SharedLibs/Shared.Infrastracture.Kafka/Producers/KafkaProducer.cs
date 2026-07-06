using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Shared.Infrastracture.Kafka.SerializeDeserialize;

namespace Shared.Infrastracture.Kafka.Producers
{
    internal class KafkaProducer<TMessage> : IKafkaProducer<TMessage>
    {
        private readonly IProducer<string, TMessage> _producer;

        private readonly string _topic;

        public void Dispose()
        {
            _producer.Dispose();
        }

        public async Task ProduceAsync(TMessage message, CancellationToken cancellationToken)
        {
            await _producer.ProduceAsync(_topic, new Message<string, TMessage>()
            {
                Key = "unique1",

                Value = message
            }, cancellationToken);
        }

        public KafkaProducer(IOptions<KafkaProducerSettings> kafkaSettings)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,

                Acks = Enum.Parse<Acks>(kafkaSettings.Value.Acks)
            };

            _topic = kafkaSettings.Value.Topic;

            _producer = new ProducerBuilder<string, TMessage>(config)
                .SetValueSerializer(new KafkaJsonSerializer<TMessage>())
                .Build();
        }
    }
}
