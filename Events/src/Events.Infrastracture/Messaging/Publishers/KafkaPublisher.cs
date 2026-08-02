using Confluent.Kafka;
using Events.Application;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Kafka;
using Shared.Messaging.Contracts.Bookings;
using Shared.Messaging.Contracts.Events;
using System.Text.Json;

namespace Events.Infrastracture.Messaging.Publishers
{
    internal class KafkaPublisher : IPublisher
    {
        private readonly IProducer<string, string> _producer;
        private readonly Kafka _kafka;

        public async Task PublishConfirmedAsync(
            ConfirmedBooking confirmed, 
            CancellationToken cancellationToken)
        {
            Message<string, string> message = new Message<string, string>()
            {
                Key = "key1",

                Value = JsonSerializer.Serialize(confirmed)
            };

            await _producer.ProduceAsync(nameof(ConfirmedBooking), message, cancellationToken);
        }

        public async Task PublishRejectedAsync(
            RejectedBooking rejected,
            CancellationToken cancellationToken)
        {
            Message<string, string> message = new Message<string, string>()
            {
                Key = "key1",

                Value = JsonSerializer.Serialize(rejected)
            };

            await _producer.ProduceAsync(nameof(RejectedBooking), message, cancellationToken);
        }

        public async Task PublishEventDeletedAsync(
            DeletedEvent deleted,
            CancellationToken cancellationToken)
        {
            Message<string, string> message = new Message<string, string>()
            {
                Key = "key1",

                Value = JsonSerializer.Serialize(deleted)
            };

            await _producer.ProduceAsync(nameof(DeletedEvent), message, cancellationToken);
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
