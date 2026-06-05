using EventManager.Infrastructure.PostgreSQL;
using EventManager.Middleware;
using EventManager.Services;
using EventManager.Handlers;

namespace EventManager
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHandlers();
            services.AddBackgroundServices();
            services.AddSingletonServices();
            services.AddPostgreDependencies();

            return services;
        }
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<CustomExceptionMiddleware>();
    }
}
