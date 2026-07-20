using Events.Application;
using Events.Application.Repositories.Cache;
using Events.Application.Repositories.Events;
using Events.Application.Repositories.Messages;
using Events.Application.Repositories.OutboxMessages;
using Events.Infrastracture.Messaging.Consumers;
using Events.Infrastracture.Messaging.Publishers;
using Events.Infrastracture.Repositories.Cache;
using Events.Infrastracture.Repositories.Events;
using Events.Infrastracture.Repositories.InboxMessages;
using Events.Infrastracture.Repositories.OutboxMessages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Kafka;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace Events.Infrastracture
{
    public static class DependenciesInjection
    {
        public static async Task<IServiceCollection> AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddRepositories();
            services.AddDbContext(configuration);

            services.AddHostedService<TopicInitializer>();

            services.AddConsumers();
            services.AddPublishers();

            await services.AddRedis(configuration);

            return services;
        }

        private static IServiceCollection AddConsumers(this IServiceCollection services)
        {

            services.AddHostedService<PendingBookingsConsumer>();
            services.AddHostedService<CancelledBookingsConsumer>();

            return services;
        }

        private static IServiceCollection AddPublishers(this IServiceCollection services)
        {
            services.AddSingleton<IPublisher, KafkaPublisher>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReadEventsRepository, PostgreReadEventsRepository>();
            services.AddScoped<IWriteEventsRepository, PostgreWriteEventsRepository>();

            services.AddScoped<IOutboxConfirmedMessagesRepository, PostgreOutboxConfirmedMessagesRepository>();

            services.AddScoped<IInboxMessagesRepository<PendingBooking>, PostgreInboxPendingMessagesRepository>();
            services.AddScoped<IInboxMessagesRepository<CancelledBooking>, PostgreInboxCancelledMessagesRepository>();

            return services;
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EventsDbContext>((options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });


            return services;
        }

        private static async Task<IServiceCollection> AddRedis(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            IConfigurationSection redisSection = configuration.GetRequiredSection("RedisOptions");

            var redisOptions = new ConfigurationOptions()
            {
                ConnectTimeout = int.Parse(redisSection.GetRequiredSection("ConnectTimeout").Value),

                SyncTimeout = int.Parse(redisSection.GetRequiredSection("SyncTimeout").Value),

                AbortOnConnectFail = bool.Parse(redisSection.GetRequiredSection("AbortOnConnectFail").Value)
            };

            redisOptions.EndPoints.Add(redisSection.GetRequiredSection("EndPoint").Value);

            services.AddSingleton<IConnectionMultiplexer>(
                await ConnectionMultiplexer.ConnectAsync(redisOptions)   
                );

            services.AddScoped<ICacheRepository, RedisRepository>();

            return services;
        }
    }
}
