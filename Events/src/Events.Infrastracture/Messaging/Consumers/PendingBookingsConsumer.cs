using Confluent.Kafka;
using Events.Application;
using Events.Application.Repositories.Cache;
using Events.Application.Repositories.Events;
using Events.Application.Repositories.Messages;
using Events.Application.Repositories.OutboxMessages;
using Events.Application.Singleton.Cache.Options;
using Events.Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Options.Global;
using System.Text.Json;

namespace Events.Infrastracture.Messaging.Consumers
{
    internal class PendingBookingsConsumer : BackgroundService
    {
        private readonly CacheKeysOptions _cacheKeysOptions;
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
                                    var confirmedOutboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxConfirmedMessagesRepository>();

                                    if (await confirmedOutboxRepository.GetActiveCountAsync(pendingBooking.UserId.Value, stoppingToken) < 10)
                                    {
                                        var scoped = _serviceScopeFactory.CreateScope();

                                        var writeEventsRepository = scoped.ServiceProvider.GetRequiredService<IWriteEventsRepository>();
                                        var cacheRepository = scoped.ServiceProvider.GetRequiredService<ICacheRepository>();

                                        @event.ReverseSeats();

                                        await writeEventsRepository.UpdateAvaliableSeats(@event.Id, @event.AvailableSeats, stoppingToken);
                                        await cacheRepository.RemoveAsync(_cacheKeysOptions.GetEventKey.FormatKey(@event.Id), stoppingToken);

                                        ConfirmedBooking confirmedBooking = new ConfirmedBooking
                                        {
                                            Id = Guid.NewGuid(),

                                            EventId = pendingBooking.EventId,

                                            BookingId = pendingBooking.BookingId,

                                            OccurredAt = DateTime.UtcNow,

                                            UserId = pendingBooking.UserId
                                        };

                                        await confirmedOutboxRepository.AddAsync(confirmedBooking, stoppingToken);

                                        await _publisher.PublishConfirmedAsync(
                                            confirmedBooking, 
                                            
                                            stoppingToken
                                        );
                                    }
                                    else
                                    {
                                        await _publisher.PublishRejectedAsync(
                                                new RejectedBooking()
                                                {
                                                    Id = Guid.NewGuid(),

                                                    EventId = pendingBooking.EventId,

                                                    BookingId = pendingBooking.BookingId,

                                                    UserId = pendingBooking.UserId,
                                                    
                                                    OccurredAt = DateTime.UtcNow,
                                                },

                                                stoppingToken
                                            );
                                    }
                                }
                                else
                                {
                                    RejectedBooking rejectedBooking = new RejectedBooking
                                    {
                                        Id = Guid.NewGuid(),

                                        BookingId = pendingBooking.BookingId,

                                        EventId = pendingBooking.EventId,

                                        OccurredAt = DateTime.UtcNow,

                                        UserId = pendingBooking.UserId
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

                                    OccurredAt = DateTime.UtcNow,

                                    UserId = pendingBooking.UserId
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

                                    OccurredAt = DateTime.UtcNow,

                                    UserId = pendingBooking.UserId
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
            IOptions<CacheKeysOptions> options,
            IPublisher publisher,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<PendingBookingsConsumer> logger)
        {
            _cacheKeysOptions = options.Value;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _publisher = publisher;
        }
    }
}
