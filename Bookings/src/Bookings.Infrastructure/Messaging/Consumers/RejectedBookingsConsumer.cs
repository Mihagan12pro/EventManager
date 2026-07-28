using Bookings.Application.Repositories;
using Bookings.Domain.Enums;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Options;
using System.Text.Json;

namespace Bookings.Infrastructure.Messaging.Consumers
{
    internal class RejectedBookingsConsumer : BackgroundService
    {
        private readonly ILogger<RejectedBookingsConsumer> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly KafkaOptions _kafkaOptions = new KafkaOptions();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServers,
                GroupId = "bookings-service",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(nameof(RejectedBooking));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    if (consumeResult?.Message?.Value == null)
                        continue;

                    var confirmedBooking = JsonSerializer.Deserialize<RejectedBooking>(consumeResult.Message.Value);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        try
                        {
                            IBookingRepository bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();

                            await bookingRepository.ChangeBookingStatusAsync(
                                confirmedBooking.BookingId,

                                BookingStatus.Rejected,

                                confirmedBooking.OccurredAt,

                                stoppingToken
                            );
                        }
                        finally
                        {

                        }
                    }


                    consumer.Commit(consumeResult);
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogInformation("The operation had been cancelled!");
                }
            }
        }

        public RejectedBookingsConsumer(
            ILogger<RejectedBookingsConsumer> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;

            _serviceScopeFactory = serviceScopeFactory;
        }
    }
}
