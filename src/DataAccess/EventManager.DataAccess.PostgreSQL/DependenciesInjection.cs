using EventManager.DataAccess.PostgreSQL.Booking;
using EventManager.DataAccess.PostgreSQL.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.DataAccess.PostgreSQL
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddPostgreDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddDbContext<AppDbContext>();

            return services;
        }
    }
}
