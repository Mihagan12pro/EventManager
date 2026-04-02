using EventManager.DTOs.Events;
using EventManager.Queues.ApplicationTasks.Booking;
using EventManager.Queues.Queues.Booking;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
        public static IServiceProvider GetProviderService()
        {
            IServiceCollection services = new ServiceCollection();

            //services.AddScoped<IEventsService, EventsService>();
            //services.AddScoped<IBookingsService, BookingsService>();
            services.AddSingleton<IEventsService, EventsService>();//Temporary solution
            services.AddSingleton<IBookingsService, BookingsService>();//Temporary solution

            services.AddSingleton<IBookingTaskQueue, MockBookingTaskQueue>();
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
                        "Только с 18 лет")
                ],

                [
                    new NewEventDto(
                        "Юбилей деда",
                        now.AddMonths(1),
                        now.AddMonths(1).AddDays(1))
                ]
            ];
        }
    }

    class MockBookingTaskQueue : IBookingTaskQueue
    {
        public void Enqueue(BookingTask task)
        {
            throw new NotImplementedException();
        }

        public bool TryDequeue(out BookingTask task)
        {
            throw new NotImplementedException();
        }
    }
}
