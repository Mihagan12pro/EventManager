using Confluent.Kafka;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Options;
using System.Text.Json;

namespace Events.Infrastracture.Messaging.Publishers
{
    internal class KafkaPublisher : IPublisher
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaOptions _kafkaOptions = new KafkaOptions();

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
            BookingRejected rejected,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
