using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;
using Shared.Messaging;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Options;

namespace Shared.Infrastructure.Kafka
{
    public class TopicInitializer : IMessagingInitializer
    {
        private readonly KafkaOptions _kafkaOptions = new KafkaOptions();

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new AdminClientConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServers
            };

            using (var adminClient = new AdminClientBuilder(config).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(
                  [
                      new TopicSpecification
                        {
                            Name = nameof(PendingBooking),

                            NumPartitions = 1,
                            
                            ReplicationFactor = 1
                        },
                   ]);
                }
                catch (CreateTopicsException ex) when (ex.Results.All(r => r.Error.Code == ErrorCode.TopicAlreadyExists))
                {
                    
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
