using Bookings.Application.Repositories;
using Bookings.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Messaging.Contracts.Bookings;

namespace Bookings.Infrastructure
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddPublishers();
            services.AdDbInteraction(configuration);

            services.AddHostedService<PendingBookingsHandler>();

            return services;
        }

        private static IServiceCollection AddPublishers(this IServiceCollection services)
        {
            //services.AddProducer<PendingBooking>("PendingBookings");

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

            services.AddScoped<IBookingRepository, PostgreBookingsRepository>();

            return services;
        }
    }
}
