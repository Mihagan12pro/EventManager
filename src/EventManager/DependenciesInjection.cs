using EventManager.Services.Events;

namespace EventManager
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddScoppedServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IEventService, EventService>();
        }
    }
}
