using Confluent.Kafka;
using Events.Application.Repositories.Events;
using Events.Application.Repositories.InboxMessages;
using Events.Domain.Exceptions;
using Events.Infrastracture.Messaging.Publishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Messaging;
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
                        var messagesRepository = scope.ServiceProvider.GetRequiredService<InboxMessagesRepository>();

                        bool result = await messagesRepository.FindMessageAsync(Guid.Parse(pendingBooking.Id), stoppingToken);

                        if (!result)
                        {
                            IReadEventsRepository readEventsRepository = scope.ServiceProvider.GetRequiredService<IReadEventsRepository>();

                            try
                            {
                                var @event = await readEventsRepository.GetEventAsync(Guid.Parse(pendingBooking.EventId), stoppingToken);

                                @event.ReverseSeats();

                                ConfirmedBooking confirmedBooking = new ConfirmedBooking(
                                    pendingBooking.Id, 
                                    
                                    pendingBooking.EventId,
                                    
                                    pendingBooking.BookingId, 

                                    DateTime.UtcNow.ToString()
                               );
                                
                                await _publisher.PublishConfirmedAsync(confirmedBooking, stoppingToken);
                            }
                            catch (InvalidOperationException ex)
                            {
                                _logger.LogInformation(
                                    "Event with id = {id} does not exists!",
                                    pendingBooking.EventId);
                            }
                            catch(NoAvailableSeatsException ex)
                            {
                                _logger.LogInformation(
                                    "Event with id = {id} has no avaliable seats!",
                                    pendingBooking.EventId);
                            }
                            finally
                            {
                                await messagesRepository.AddMessageAsync(
                                    new Message()
                                    {
                                        Id = Guid.Parse(pendingBooking.Id) 
                                    },
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
