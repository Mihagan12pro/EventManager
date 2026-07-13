using Events.Application;
using Events.Application.Repositories.Events;
using Events.Application.Repositories.InboxMessages;
using Events.Infrastracture.Messaging.Consumers;
using Events.Infrastracture.Messaging.Publishers;
using Events.Infrastracture.Repositories.Events;
using Events.Infrastracture.Repositories.InboxMessages;
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

            services.AddHostedService<PendingBookingsConsumer>();
            services.AddHostedService<CancelledBookingsConsumer>();

            services.AddSingleton<IPublisher, KafkaPublisher>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReadEventsRepository, PostgreReadEventsRepository>();
            services.AddScoped<IWriteEventsRepository, PostgreWriteEventsRepository>();

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
