using Bookings.Application.Publishers;
using Bookings.Application.Repositories;
using Bookings.Infrastructure.Messaging.Consumers;
using Bookings.Infrastructure.Messaging.Publishers;
using Bookings.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Kafka;

namespace Bookings.Infrastructure
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddHostedServices();

            services.AddPublishers();
            services.AddConsumers();

            services.AdDbInteraction(configuration);

            return services;
        }

        private static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<TopicInitializer>();

            return services;
        }

        private static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddHostedService<ConfirmedBookingsConsumer>();

            return services;
        }

        private static IServiceCollection AddPublishers(this IServiceCollection services)
        {
            services.AddSingleton<IPublisher, KafkaPublisher>();

            return services;
        }

        private static IServiceCollection AdDbInteraction(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<BookingsDbContext>((options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IInboxMessagesRepository, PostgreInboxMessagesRepository>();
            services.AddScoped<IBookingRepository, PostgreBookingsRepository>();

            return services;
        }
    }
}
