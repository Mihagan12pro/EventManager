using EventManager.DataAccess.PostgreSQL.Booking;
using EventManager.DataAccess.PostgreSQL.DbContexts;
using EventManager.DataAccess.PostgreSQL.Events;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventManager.Tests.Integration
{
    public class EventManagerAppFactory<TEntryPoint> 
        : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                ILoggingBuilder loggingBuilder;
                loggingBuilder.max

                services.AddLogging();

                services.AddDbContext<AppDbContextBase, DockerAppDbContext>();

                services.AddScoped<IEventsRepository, PostgreEventsRepository>();
                services.AddScoped<IBookingsRepository, PostgreBookingsRepository>();

                services.AddScoped<IEventsService, EventsService>();
                services.AddScoped<IBookingsService, BookingsService>();

                services.AddHostedService<BookingHandlingService>();
            });

            builder.UseEnvironment("Development");
        }
    }
}
