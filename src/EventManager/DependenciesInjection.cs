using EventManager.Middleware;
using EventManager.TasksManagers;
using EventsManager.Services;

namespace EventManager
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScopedServices()
                .AddBackgroundServices()
                    .AddQueues();
        }

        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
