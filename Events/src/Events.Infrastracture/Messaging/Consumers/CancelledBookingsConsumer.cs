using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Options;

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
