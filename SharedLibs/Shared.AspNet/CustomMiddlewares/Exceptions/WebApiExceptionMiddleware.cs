using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Failures.Errors.Factories;
using Shared.Failures.Exceptions.WebApi;
using Shared.Failures.Exceptions.WebApi.ClientErrors;
using HttpError = Shared.Failures.HttpError;

namespace Shared.AspNet.CustomMiddlewares.Exceptions
{
    public class WebApiExceptionMiddleware : CustomMiddleware
    {
        private readonly ILogger<WebApiExceptionMiddleware> _logger;

        public override async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch(UniqueConstraitException ex)
            {
                LogWarning(ex, httpContext);

                await ModifyResponse(httpContext, ex.Error);
            }
            catch (WebApiException ex)
                when (ex is UniqueConstraitException
                    || ex is NotFoundException
                        || ex is ForbiddenException)
            {
                LogWarning(ex, httpContext);

                await ModifyResponse(httpContext, ex.Error);
            }
            catch (WebApiException ex)
                when (ex is BadRequestException)
            {
                LogInformation(ex, httpContext);

                await ModifyResponse(httpContext, ex.Error);
            }
            catch (ArgumentNullException ex)
            {
                LogError(ex, httpContext);

                await ModifyResponse(httpContext, ClientErrorsFactory.NotFoundWorkbench.Craft("Not found!"));
            }
            catch (InvalidOperationException ex)
            {
                LogError(ex, httpContext);

                await ModifyResponse(httpContext, ClientErrorsFactory.NotFoundWorkbench.Craft("Not found!"));
            }
            catch (Exception ex)
            {
                LogError(ex, httpContext);

                await ModifyResponse(httpContext, ServerErrorsFactory.InternalServerErrorWorkbench.Craft("Internal server error!"));
            }
        }

        private void LogInformation(Exception ex, HttpContext httpContext)
        {
            _logger.LogInformation(
                "Unhandled exception: {message}. Method={Method}, Path={Path}, RequestId={RequestId}",
                ex.Message,
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.Request.Headers["x-request-id"],
                httpContext.TraceIdentifier);
        }

        private void LogWarning(Exception ex, HttpContext httpContext)
        {
            _logger.LogWarning(
               "Unhandled exception: {message}. Method={Method}, Path={Path}, TraceId={TraceId}",
               ex.Message,
               httpContext.Request.Method,
               httpContext.Request.Path,
               httpContext.TraceIdentifier);
        }

        private void LogError(Exception ex, HttpContext httpContext)
        {
            _logger.LogError(
                "Unhandled exception: {message}. Method={Method}, Path={Path}, TraceId={TraceId}",
                ex.Message,
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.TraceIdentifier);
        }

        private void LogCritical(Exception ex, HttpContext httpContext)
        {
            _logger.LogCritical(
                ex,
                "Unhandled exception. Method={Method}, Path={Path}, TraceId={TraceId}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.TraceIdentifier);
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
