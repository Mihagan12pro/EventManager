using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Shared.Messaging;
using Shared.Messaging.Contracts.Bookings;

namespace Shared.Infrastructure.Kafka
{
    public class TopicInitializer : IMessagingInitializer
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new AdminClientConfig
            {
                BootstrapServers ="localhost:9092"
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
                catch
                {

                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
