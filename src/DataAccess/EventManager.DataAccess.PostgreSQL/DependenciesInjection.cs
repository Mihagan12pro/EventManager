using EventManager.DataAccess.PostgreSQL.Booking;
using EventManager.DataAccess.PostgreSQL.DbContexts;
using EventManager.DataAccess.PostgreSQL.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace EventManager.DataAccess.PostgreSQL
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddPostgreDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<IBookingsRepository, BookingsRepository>();

            services.AddDbContext<AppDbContextBase, AppDbContext>();

            return services;
        }
    }
}
