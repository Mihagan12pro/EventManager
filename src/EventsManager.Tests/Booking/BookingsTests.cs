using EventManager.DTOs.Events;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Booking
{
    [Trait("Category", "Bookings")]
    public class BookingsTests
    {
        public static IServiceProvider GetProviderService()
        {
            IServiceCollection services = new ServiceCollection();

            //services.AddScoped<IEventsService, EventsService>();
            //services.AddScoped<IBookingsService, BookingsService>();
            services.AddSingleton<IEventsService, InMemoryEventsService>();//Temporary solution
            services.AddSingleton<IBookingsService, InMemoryBookingsService>();//Temporary solution

            services.AddHostedService<BookingHandlingService>();

            services.AddLogging();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        public static IEnumerable<object[]> AddEvents()
        {
            DateTime now = DateTime.Now.AddDays(1);

            return
            [
                [
                    new NewEventDto(
                        "Вечеринка",
                        now,
                        now.AddHours(10), 
                        10,
                        "Только с 18 лет")
                ],

                [
                    new NewEventDto(
                        "Юбилей деда",
                        now.AddMonths(1),
                        now.AddMonths(1).AddDays(1),
                        10)
                ]
            ];
        }
    }
}
