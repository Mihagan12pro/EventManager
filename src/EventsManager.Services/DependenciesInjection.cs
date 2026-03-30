using EventManager.Queues;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Services
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection scopedServices)
        {
            //scopedServices.AddScoped<IBookingService, BookingService>();
            //scopedServices.AddScoped<IEventsService, EventsService>();

            scopedServices.AddSingleton<IBookingService, BookingService>();//It is a temporary solution!
            scopedServices.AddSingleton<IEventsService, EventsService>();//It is a temporary solution!

            return scopedServices;
        }

        public static IServiceCollection AddBackgroundServices(this IServiceCollection backgroundServices)
        {
            backgroundServices.AddHostedService<BookingHandlingService>();

            return backgroundServices;
        }

        public static IServiceCollection AddSingletonServices(this IServiceCollection singletonServices)
        {
            singletonServices.AddQueues();

            return singletonServices;
        }
    }
}
