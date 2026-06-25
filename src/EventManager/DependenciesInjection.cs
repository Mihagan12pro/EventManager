using EventManager.Infrastructure.PostgreSQL;
using EventManager.Application;
using EventManager.Infrastructure.Security;
using EventManager.Middleware.Exceptions;
using EventManager.Infrastructure.PostgreSQL.DbContexts;

namespace EventManager
{
    public static class DependenciesInjection
    {
        private static IHostApplicationBuilder _hostApplicationBuilder;

        public static IServiceCollection AddServices(this IServiceCollection services, IHostApplicationBuilder hostApplicationBuilder)
        {
            _hostApplicationBuilder = hostApplicationBuilder;

            services.AddHttpContextAccessor();

            services.AddHandlers();
            services.AddSecurity();
            services.AddBackgroundServices();
            services.AddSingletonServices();

            if (!hostApplicationBuilder.Environment.IsEnvironment("Testing"))
            {
                services.AddPostgreDependencies();
            }

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
