using Confluent.Kafka;
using Shared.Messaging.Contracts.Bookings;
using System.Text.Json;

namespace Bookings.Infrastructure.Messaging.Publishers
{
    internal class KafkaPublisher : IPublisher
    {
        private readonly IProducer<string, string> _producer;

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
                BootstrapServers ="localhost:9092"
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }
    }
}
