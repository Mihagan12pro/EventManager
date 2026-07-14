using Events.Application;
using Events.Application.Repositories.Events;
using Events.Application.Repositories.Messages;
using Events.Application.Repositories.OutboxMessages;
using Events.Infrastracture.Messaging.Consumers;
using Events.Infrastracture.Messaging.Publishers;
using Events.Infrastracture.Repositories.Events;
using Events.Infrastracture.Repositories.InboxMessages;
using Events.Infrastracture.Repositories.OutboxMessages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Kafka;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Options;

namespace Events.Infrastracture
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddRepositories();
            services.AddDbContext(configuration);

            services.AddHostedService<TopicInitializer>();

            services.AddConsumers();
            services.AddPublishers();

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
    }
}
