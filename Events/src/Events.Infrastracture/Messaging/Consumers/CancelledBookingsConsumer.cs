using Confluent.Kafka;
using Events.Application.Repositories.Cache;
using Events.Application.Repositories.Events;
using Events.Application.Repositories.Messages;
using Events.Application.Repositories.OutboxMessages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Failures.Exceptions.WebApi.ClientErrors;
using Shared.Failures.Exceptions.WebApi.ServerErrors;
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
                    
                    if (cancelledBooking == null)
                        throw new InternalServerErrorException();

                    using (var scoped = _serviceScopeFactory.CreateScope())
                    {
                        var inboxRepository = scoped.ServiceProvider.GetRequiredService<IInboxMessagesRepository<CancelledBooking>>();
                        
                        if (!await inboxRepository.FindMessageAsync(cancelledBooking, stoppingToken))
                        {
                            try
                            {
                                var writeEventsRepository = scoped.ServiceProvider.GetRequiredService<IWriteEventsRepository>();
                                var readEventsRepository = scoped.ServiceProvider.GetRequiredService<IReadEventsRepository>();
                                var cacheRepository = scoped.ServiceProvider.GetRequiredService<ICacheRepository>();

                                var @event = await readEventsRepository.GetEventAsync(cancelledBooking.EventId, stoppingToken);
                                @event.ReleaseSeats();

                                await writeEventsRepository.UpdateAvaliableSeats(@event.Id, @event.AvailableSeats, stoppingToken);
                                await cacheRepository.RemoveAsync($"events:event:{@event.Id}", stoppingToken);

                                var outboxRepository = scoped.ServiceProvider.GetRequiredService<IOutboxConfirmedMessagesRepository>();
                                await outboxRepository.DeleteAsync(cancelledBooking.BookingId, stoppingToken);
                            }
                            catch(InvalidOperationException ex)
                            {
                                _logger.LogInformation(
                                   "Event with id = {id} does not exists!",
                                   cancelledBooking.EventId);
                            }
                            catch(ConflictException ex)
                            {
                                _logger.LogInformation(
                                   "Event with id = {id} has no reversed seats!",
                                   cancelledBooking.EventId);
                            }
                            finally
                            {
                                await inboxRepository.AddMessageAsync(cancelledBooking, stoppingToken);
                            }
                        }
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
