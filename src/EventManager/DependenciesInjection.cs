using EventManager.Infrastructure.PostgreSQL;
using EventManager.Application;
using EventManager.Infrastructure.Security;
using EventManager.Middleware.Exceptions;

namespace EventManager
{
    public static class DependenciesInjection
    {
        private static IHostApplicationBuilder _hostApplicationBuilder;

        public static IServiceCollection AddServices(
            this IServiceCollection services,
            IHostApplicationBuilder hostApplicationBuilder,
            IConfiguration configuration)
        {
            _hostApplicationBuilder = hostApplicationBuilder;

            services.AddHttpContextAccessor();

            services.AddHandlers();
            services.AddSecurity();
            services.AddHostedServices();
            services.AddSingletonServices();

            if (!hostApplicationBuilder.Environment.IsEnvironment("Testing"))
            {
                services.AddPostgreDependencies(configuration);
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
