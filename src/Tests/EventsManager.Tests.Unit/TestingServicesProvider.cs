using EventManager.DataAccess.PostgreSQL.Booking;
using EventManager.DataAccess.PostgreSQL.DbContexts;
using EventManager.DataAccess.PostgreSQL.Events;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Unit
{
    internal static class TestingServicesProvider
    {
        public static IServiceProvider GetServicesProvider()
        {
            IServiceCollection services = new ServiceCollection();

            var dbName = Guid.NewGuid().ToString();

            services.AddDbContext<AppDbContextBase, InMemoryAppDbContext>(options =>
                options.UseInMemoryDatabase(dbName));

            services.AddValidatorsFromAssembly(typeof(Services.DependenciesInjection).Assembly);

            services.AddScoped<IEventsService, EventsService>();
            services.AddScoped<IBookingsService, BookingsService>();

            services.AddScoped<IEventsRepository, PostgreEventsRepository>();
            services.AddScoped<IBookingsRepository, PostgreBookingsRepository>();

            services.AddHostedService<BookingHandlingService>();

            services.AddLogging();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
