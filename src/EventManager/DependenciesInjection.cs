using EventManager.Infrastructure.PostgreSQL;
using EventManager.Middleware;
using EventManager.Application;

namespace EventManager
{
    public static class DependenciesInjection
    {
        private static IConfiguration configuration;

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHandlers();
            services.AddBackgroundServices();
            services.AddSingletonServices();
            services.AddPostgreDependencies(configuration);

            return services;
        }
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<CustomExceptionMiddleware>();
    }
}
