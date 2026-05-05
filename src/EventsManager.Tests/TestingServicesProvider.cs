using EventManager.DataAccess.PostgreSQL;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Services.Tests
{
    internal static class TestingServicesProvider
    {
        public static IServiceProvider GetProviderService()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddScoped<IEventsService, EventsService>();
            services.AddScoped<IBookingsService, BookingsService>();

            services.AddHostedService<BookingHandlingService>();

            services.AddLogging();

            var dbName = Guid.NewGuid().ToString();
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(dbName));

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
