using Confluent.Kafka;
using Events.Application;
using Events.Application.Repositories.Events;
using Events.Application.Repositories.InboxMessages;
using Events.Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Options;
using System.Text.Json;

namespace Events.Infrastracture.Messaging.Consumers
{
    internal class PendingBookingsConsumer : BackgroundService
    {
        private readonly ILogger<PendingBookingsConsumer> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly KafkaOptions kafkaOptions = new KafkaOptions();

        private readonly IPublisher _publisher;

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

            consumer.Subscribe(nameof(PendingBooking));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    if (consumeResult?.Message?.Value == null)
                        continue;

                    var pendingBooking = JsonSerializer.Deserialize<PendingBooking>(consumeResult.Message.Value);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var messagesRepository = scope.ServiceProvider.GetRequiredService<IInboxMessagesRepository<PendingBooking>>();

                        bool result = await messagesRepository.FindMessageAsync(pendingBooking, stoppingToken);

                        if (!result)
                        {
                            IReadEventsRepository readEventsRepository = scope.ServiceProvider
                                .GetRequiredService<IReadEventsRepository>();

                            try
                            {
                                var @event = await readEventsRepository.GetEventAsync(pendingBooking.EventId, stoppingToken);

                                DateTime now = DateTime.UtcNow;

                                if (@event.StartAt > now)
                                {
                                    @event.ReverseSeats();

                                    ConfirmedBooking confirmedBooking = new ConfirmedBooking
                                    {
                                        Id = Guid.NewGuid(),

                                        EventId = pendingBooking.EventId,

                                        BookingId = pendingBooking.BookingId,

                                        OccurredAt = DateTime.UtcNow
                                    };

                                    await _publisher.PublishConfirmedAsync(confirmedBooking, stoppingToken);
                                }
                                else
                                {
                                    RejectedBooking rejectedBooking = new RejectedBooking
                                    {
                                        Id = Guid.NewGuid(),

                                        BookingId = pendingBooking.BookingId,

                                        EventId = pendingBooking.EventId,

                                        OccurredAt = DateTime.UtcNow
                                    };

                                    await _publisher.PublishRejectedAsync(
                                        rejectedBooking,

                                        stoppingToken
                                    );

                                    _logger.LogInformation(
                                        "Event with id = {id} had been already started!",
                                        pendingBooking.EventId);
                                }
                            }
                            catch (InvalidOperationException ex)
                            {
                                RejectedBooking rejectedBooking = new RejectedBooking
                                {
                                    Id = Guid.NewGuid(),

                                    BookingId = pendingBooking.BookingId,

                                    EventId = pendingBooking.EventId,

                                    OccurredAt = DateTime.UtcNow
                                };

                                await _publisher.PublishRejectedAsync(
                                    rejectedBooking,

                                    stoppingToken
                                );

                                _logger.LogInformation(
                                    "Event with id = {id} does not exists!",
                                    pendingBooking.EventId);
                            }
                            catch(NoAvailableSeatsException ex)
                            {
                                RejectedBooking rejectedBooking = new RejectedBooking 
                                {
                                    Id = Guid.NewGuid(),

                                    BookingId = pendingBooking.BookingId,

                                    EventId = pendingBooking.EventId,

                                    OccurredAt = DateTime.UtcNow
                                };

                                await _publisher.PublishRejectedAsync(
                                    rejectedBooking, 
                                    
                                    stoppingToken
                                );

                                _logger.LogInformation(
                                    "Event with id = {id} has no avaliable seats!",
                                    pendingBooking.EventId);
                            }
                            finally
                            {
                                await messagesRepository.AddMessageAsync(
                                    pendingBooking,

                                    stoppingToken
                                );
                            }
                        }
                    }


                    consumer.Commit(consumeResult);
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogInformation("Operation cancelled!");
                }
            }
        }

        public PendingBookingsConsumer(
            IPublisher publisher,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<PendingBookingsConsumer> logger)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _publisher = publisher;
        }
    }
}
