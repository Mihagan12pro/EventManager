using Confluent.Kafka;
using Shared.Messaging.Contracts.Bookings;
using Bookings.Application.Publishers;
using System.Text.Json;
using Shared.Objects.Classes.Options;
using Shared.Infrastructure.Kafka;
using Microsoft.Extensions.Options;

namespace Bookings.Infrastructure.Messaging.Publishers
{
    internal class KafkaPublisher : IPublisher
    {
        private readonly IProducer<string, string> _producer;
        private readonly Kafka _kafka;

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

        public async Task ProduceAsync(
            CancelledBooking cancelledBooking, 
            CancellationToken cancellationToken)
        {
            Message<string, string> message = new Message<string, string>()
            {
                Key = "key1",

                Value = JsonSerializer.Serialize(cancelledBooking)
            };

            await _producer.ProduceAsync(nameof(CancelledBooking), message, cancellationToken);
        }

        public KafkaPublisher(IOptions<Kafka> kafkaOptions)
        {
            _kafka = kafkaOptions.Value;

            var config = new ProducerConfig
            {
                BootstrapServers = _kafka.BootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }
    }
}
