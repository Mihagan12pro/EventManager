using Microsoft.AspNetCore.Builder;
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

        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<WebApiExceptionMiddleware>();
            app.UseMiddleware<DbExceptionsMiddleware>();

            return app;
        }
    }
}
