using Bookings.Application.Repositories;
using Bookings.Domain;
using Bookings.Domain.Enums;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Failures.Exceptions.WebApi.ServerErrors;
using Shared.Messaging.Contracts.Events;
using Shared.Objects.Classes.Collections;
using Shared.Objects.Classes.Options.Global;
using System.Text.Json;

namespace Bookings.Infrastructure.Messaging.Consumers
{
    internal class DeletedEventsConsumer : BackgroundService
    {
        private readonly ILogger<DeletedEventsConsumer> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly KafkaOptions kafkaOptions = new KafkaOptions();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaOptions.BootstrapServers,
                GroupId = "bookings-service",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(nameof(DeletedEvent));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    if (consumeResult?.Message?.Value == null)
                        continue;

                    DeletedEvent deletedEvent = JsonSerializer.Deserialize<DeletedEvent>(consumeResult.Message.Value);

                    if (deletedEvent == null) 
                        throw new InternalServerErrorException();

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        try
                        {
                            IBookingRepository bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();

                            var bookings = await bookingRepository.GetAllWithFiltersAsync(
                                new Filters<Booking>()
                                    {
                                        (Booking b) => (b.Status ==  BookingStatus.Confirmed || b.Status == BookingStatus.Pending)
                                                        && b.EventId == deletedEvent.EventId
                                    },
                                stoppingToken
                            );

                            foreach(var booking in bookings.ToList())
                            {
                                await bookingRepository.ChangeBookingStatusAsync(
                                    booking.Id,

                                    BookingStatus.Cancelled,

                                    DateTime.UtcNow,

                                    stoppingToken
                                );
                            }
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

        public DeletedEventsConsumer(
            ILogger<DeletedEventsConsumer> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;

            _serviceScopeFactory = serviceScopeFactory;
        }
    }
}
