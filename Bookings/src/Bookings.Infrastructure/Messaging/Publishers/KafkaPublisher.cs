using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Shared.Messaging.Contracts.Bookings;

namespace Bookings.Infrastructure.Messaging.Publishers
{
    internal class KafkaPublisher : IPublisher
    {
        private readonly IProducer<string, string> _producer;

        public async Task<IPublisher> ProduceAsync(
            PendingBooking pendingBooking, 
            CancellationToken cancellationToken)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = ""
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }
    }
}
