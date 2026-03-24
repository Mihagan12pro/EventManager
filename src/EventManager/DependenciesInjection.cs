using EventManager.Middleware;
using EventManager.Services.Events;

namespace EventManager
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddScopedDependencies(this IServiceCollection services)
        {
            return services.AddScopedServices();
        }

        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomExceptionMiddleware>();
        }

        private static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            return services
               .AddSingleton<IEventsService, EventsService>();//Making IEventsService singleton is a temporary solution!
        }
    }
}
