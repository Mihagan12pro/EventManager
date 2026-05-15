using EventManager.DataAccess.PostgreSQL;
using EventManager.DataAccess.PostgreSQL.Booking;
using EventManager.DataAccess.PostgreSQL.DbContexts;
using EventManager.DataAccess.PostgreSQL.Events;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Services.Tests
{
    internal static class TestingServicesProvider
    {
        public static IServiceProvider GetServicesProvider()
        {
            IServiceCollection services = new ServiceCollection();

            var dbName = Guid.NewGuid().ToString();

            services.AddDbContext<AppDbContextBase, InMemoryAppDbContext>(options =>
                options.UseInMemoryDatabase(dbName));

            services.AddValidatorsFromAssembly(typeof(EventManager.Services.DependenciesInjection).Assembly);

            services.AddScoped<IEventsService, EventsService>();
            services.AddScoped<IBookingsService, BookingsService>();

            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<IBookingsRepository, BookingsRepository>();

            services.AddHostedService<BookingHandlingService>();

            services.AddLogging();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
