using EventManager.Services.Events;

namespace EventManager
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddScopedDependencies(this IServiceCollection services)
        {
            return services.AddScopedServices();
        }

        private static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            return services
               .AddScoped<IEventsService, EventsService>();
        }
    }
}
