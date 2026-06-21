using EventManager.Infrastructure.PostgreSQL;
using EventManager.Application;
using EventManager.Infrastructure.Security;
using EventManager.Middleware.Exceptions;

namespace EventManager
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHandlers();
            services.AddSecurity();
            services.AddBackgroundServices();
            services.AddSingletonServices();
            services.AddPostgreDependencies();

            return services;
        }
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<WebApiExceptionMiddleware>();
            app.UseMiddleware<DbExceptionsMiddleware>();

            return app;
        }
    }
}
