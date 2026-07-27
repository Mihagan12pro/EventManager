using Microsoft.AspNetCore.Http;
using Shared.Failures.Exceptions.WebApi.ClientErrors;
using Shared.Failures.Exceptions.WebApi.ServerErrors;

namespace Shared.AspNet.CustomMiddlewares.Exceptions
{
    public class SystemToWebApiExceptionsMiddleware : CustomMiddleware
    {
        public override async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(InvalidOperationException ex)
            {
                throw new NotFoundException(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ex.Message);
            }
        }

        public SystemToWebApiExceptionsMiddleware(RequestDelegate next) : base(next)
        {

        }
    }
}