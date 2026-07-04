using Microsoft.AspNetCore.Builder;
using Shared.AspNet.CustomMiddlewares.Exceptions;
using Shared.Failures.Exceptions.WebApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.AspNet.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomWebApiExceptionsMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<WebApiExceptionMiddleware>();

            return applicationBuilder;
        }

        public static IApplicationBuilder UseCustomDbExceptionsMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<DbExceptionsMiddleware>();

            return applicationBuilder;
        }
    }
}
