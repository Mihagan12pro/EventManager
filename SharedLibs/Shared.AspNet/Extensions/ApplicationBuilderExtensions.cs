using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.AspNet.CustomMiddlewares.Exceptions;

namespace Shared.AspNet.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerForDebugging(this IApplicationBuilder app)
        {
            if (app is WebApplication webApp)
            {
                if (webApp.Environment.IsDevelopment())
                {
                    webApp.UseSwagger();
                    webApp.UseSwaggerUI();
                }
            }

            return app;
        }

        /// <summary>
        /// Registers WebApiExceptionMiddleware and DbExceptionsMiddleware
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<WebApiExceptionMiddleware>();
            app.UseMiddleware<DbExceptionsMiddleware>();

            return app;
        }

        public static IApplicationBuilder UseTelemetry(this IApplicationBuilder app)
        {
            if (app is WebApplication webApp)
            {
                webApp.MapPrometheusScrapingEndpoint();
            }

            return app;
        }

        public static IApplicationBuilder UseCustomHttpsRedirection(this IApplicationBuilder app)
        {
            if (app is WebApplication webApp)
            {
                app.UseWhen(context => !context.Request.Path.StartsWithSegments("/metrics"),
                    appBuilder =>
                    {
                        appBuilder.UseHttpsRedirection();
                    }
                );
            }

            return app;
        }
    }
}
