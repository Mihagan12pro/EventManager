using Microsoft.AspNetCore.Http;

namespace Shared.AspNet.CustomMiddlewares
{
    public abstract class CustomMiddleware
    {
        protected readonly RequestDelegate next;

        public abstract Task InvokeAsync(HttpContext context);

        public CustomMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
    }
}
