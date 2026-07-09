using Events.Application.Repositories;
using Events.Infrastracture.Messaging.Consumers;
using Events.Infrastracture.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Kafka;

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

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReadEventsRepository, PostgreReadEventsRepository>();
            services.AddScoped<IWriteEventsRepository, PostgreWriteEventsRepository>();

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
