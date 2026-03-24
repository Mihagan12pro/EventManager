using EventManager.Exceptions;
using EventManager.Shared;

namespace EventManager.Middleware
{
    public class CustomExceptionMiddleware : CustomMiddleware
    {
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public override async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (WebApiException ex)
            {
                LogError(ex, httpContext);

                await ModifyResponse(httpContext, ex.Error);
            }
            catch (Exception ex)
            {
                LogError(ex, httpContext);

                await ModifyResponse(httpContext, Error.CreateError500());
            }
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

        private async Task ModifyResponse(HttpContext httpContext, Error error)
        {
            HttpResponse response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)error.StatusCode;

            await httpContext.Response.WriteAsJsonAsync(error);
        }

        public CustomExceptionMiddleware(
            RequestDelegate next,
            ILogger<CustomExceptionMiddleware> logger) : base(next)
        {
            _logger = logger;
        }
    }
}
