using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Messaging;
using Shared.Messaging.Contracts.Bookings;
using Shared.Messaging.Contracts.Events;

namespace Shared.Infrastructure.Kafka
{
    public class TopicInitializer : IMessagingInitializer
    {
        private readonly Kafka _kafka;

        private readonly ILogger<TopicInitializer> _logger;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new AdminClientConfig
            {
                BootstrapServers = _kafka.BootstrapServers
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

                        new TopicSpecification
                        {
                            Name = nameof(ConfirmedBooking),

                            NumPartitions = 1,

                            ReplicationFactor = 1
                        },

                        new TopicSpecification
                        {
                            Name = nameof(RejectedBooking),

                            NumPartitions = 1,

                            ReplicationFactor = 1
                        },

                        new TopicSpecification
                        {
                            Name = nameof(CancelledBooking),

                            NumPartitions = 1,

                            ReplicationFactor = 1
                        },

                        new TopicSpecification
                        {
                            Name = nameof(DeletedEvent),

                            NumPartitions = 1,

                            ReplicationFactor = 1
                        }
                    ]);
                }
                catch (CreateTopicsException ex) when (
                    ex.Results.All(
                        r => r.Error.Code == ErrorCode.TopicAlreadyExists)
                    )
                {
                    _logger.LogInformation("This topic already exists!");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public TopicInitializer(
            ILogger<TopicInitializer> logger,
            IOptions<Kafka> kafkaOption)
        {
            _kafka = kafkaOption.Value;
            _logger = logger;
        }
    }
}
