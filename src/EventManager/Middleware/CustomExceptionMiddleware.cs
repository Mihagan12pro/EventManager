namespace EventManager.Middleware
{
    public class CustomExceptionMiddleware : CustomMiddleware
    {
        public override Task InvokeAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public CustomExceptionMiddleware(RequestDelegate next) : base(next)
        {
        }
    }
}
