using EventManager.DataAccess.PostgreSQL;
using EventManager.Middleware;
using EventManager.Services;

namespace EventManager
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScopedServices()
                .AddBackgroundServices()
                    .AddSingletonServices()
                        .AddPostgreDependencies();
        }

        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
