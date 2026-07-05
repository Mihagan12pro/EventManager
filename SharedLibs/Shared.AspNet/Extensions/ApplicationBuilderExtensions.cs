using Microsoft.AspNetCore.Builder;
using Shared.AspNet.CustomMiddlewares.Exceptions;
using Shared.Failures.Exceptions.WebApi;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Shared.AspNet.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWebApiExceptionsMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<WebApiExceptionMiddleware>();

            return app;
        }

        public static IApplicationBuilder UseDbExceptionsMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<DbExceptionsMiddleware>();

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
