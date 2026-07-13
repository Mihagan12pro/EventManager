using Confluent.Kafka;
using Events.Application.Repositories.InboxMessages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Options;
using System.Text.Json;

namespace Events.Infrastracture.Messaging.Consumers
{
    internal class CancelledBookingsConsumer : BackgroundService
    {
        private readonly ILogger<CancelledBookingsConsumer> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly KafkaOptions kafkaOptions = new KafkaOptions();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaOptions.BootstrapServers,
                GroupId = "event-service",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(nameof(CancelledBooking));

            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    if (consumeResult?.Message?.Value == null)
                        continue;

                    var cancelledBooking = JsonSerializer.Deserialize<CancelledBooking>(consumeResult.Message.Value);

                    using (var scoped = _serviceScopeFactory.CreateScope())
                    {
                        var inboxRepository = scoped.ServiceProvider.GetRequiredService<IInboxMessagesRepository<CancelledBooking>>();
                        //if (await inboxRepository.FindMessageAsync(can))
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogInformation("Operation cancelled!");
                }
            }
        }

        public CancelledBookingsConsumer(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<CancelledBookingsConsumer> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;

            _logger = logger;
        }
    }
}
