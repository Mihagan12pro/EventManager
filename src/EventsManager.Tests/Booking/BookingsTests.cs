using EventManager.DTOs.Events;
using EventManager.Queues.Queues.Booking;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using EventManager.Tests.Booking.Create;
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
            services.AddSingleton<IEventsService, EventsService>();//Temporary solution
            services.AddSingleton<IBookingsService, BookingsService>();//Temporary solution

            services.AddSingleton<IBookingPendingQueue, MockBookingTaskQueue>();
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
