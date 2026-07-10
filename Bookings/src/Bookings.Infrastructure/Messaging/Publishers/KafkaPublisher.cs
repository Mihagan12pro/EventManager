using Confluent.Kafka;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Options;
using System.Text.Json;

namespace Bookings.Infrastructure.Messaging.Publishers
{
    internal class KafkaPublisher : IPublisher
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaOptions _kafkaOptions = new KafkaOptions();

        public async Task ProduceAsync(
            PendingBooking pendingBooking, 
            CancellationToken cancellationToken)
        {
            Message<string, string> message = new Message<string, string>()
            {
                Key = "key1",

                Value = JsonSerializer.Serialize(pendingBooking)
            };

            await _producer.ProduceAsync(nameof(PendingBooking), message, cancellationToken);
        }

        public KafkaPublisher()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }
    }
}
