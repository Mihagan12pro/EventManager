using EventManager.Services.Bookings;
using EventManager.Services.Events;
using EventsManager.Services.Background.Bookings;
using EventsManager.Services.Bookings;
using Microsoft.Extensions.DependencyInjection;

namespace EventsManager.Services
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
    }
}
