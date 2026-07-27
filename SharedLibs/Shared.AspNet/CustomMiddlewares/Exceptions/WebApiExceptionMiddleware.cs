using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Failures.Enums;
using Shared.Failures.Exceptions.WebApi;
using System.Net;
using HttpError = Shared.Failures.HttpError;

namespace Shared.AspNet.CustomMiddlewares.Exceptions
{
    public class WebApiExceptionMiddleware : CustomMiddleware
    {
        private readonly ILogger<WebApiExceptionMiddleware> _logger;

        public override async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (WebApiException ex) 
                when (ex.Error.ErrorType == ErrorType.Client)
            {
                switch(ex.Error.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.NotFound:
                        {
                            LogWarning(ex, context);

                            break;
                        }
                    default:
                        {
                            LogInformation(ex, context);

                            break;
                        }
                }

                await ModifyResponse(context, ex.Error);
            }
            catch (WebApiException ex) 
                when (ex.Error.ErrorType == ErrorType.Server)
            {
                LogError(ex, context);

                await ModifyResponse(context, ex.Error);
            }
        }

        private void LogInformation(Exception ex, HttpContext httpContext)
        {
            _logger.LogInformation(
                ex,
                "Unhandled exception. Method={Method}, Path={Path}, RequestId={RequestId}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.Request.Headers["x-request-id"]);
        }

        private void LogWarning(Exception ex, HttpContext httpContext)
        {
            _logger.LogWarning(
                ex,
                "Unhandled exception. Method={Method}, Path={Path}, RequestId={RequestId}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.Request.Headers["x-request-id"]);
        }

        private void LogError(Exception ex, HttpContext httpContext)
        {
            _logger.LogError(
                ex,
                "Unhandled exception. Method={Method}, Path={Path}, RequestId={RequestId}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.Request.Headers["x-request-id"]);
        }

        private async Task ModifyResponse(HttpContext httpContext, HttpError error)
        {
            HttpResponse response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)error.StatusCode;

            await response.WriteAsJsonAsync(error.Errors);
        }

        public WebApiExceptionMiddleware(
            RequestDelegate next,
            ILogger<WebApiExceptionMiddleware> logger) : base(next)
        {
            _logger = logger;
        }
    }
}
