using Confluent.Kafka;
using Events.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Messaging.Contracts.Bookings;
using System.Text.Json;

namespace Events.Infrastracture.Messaging.Consumers
{
    internal class PendingBookingsConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers ="localhost:9092",
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

                    var message = JsonSerializer.Deserialize<PendingBooking>(consumeResult.Message.Value);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        IReadEventsRepository readEventsRepository = scope.ServiceProvider.GetRequiredService<IReadEventsRepository>();

                        try
                        {
                            var @event = await readEventsRepository.GetEventAsync(Guid.Parse(message.EventId), stoppingToken);
                        }
                        catch
                        {

                        }
                    }


                    consumer.Commit(consumeResult);
                }
                catch
                {
                    
                }
            }
        }

        public PendingBookingsConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
    }
}
