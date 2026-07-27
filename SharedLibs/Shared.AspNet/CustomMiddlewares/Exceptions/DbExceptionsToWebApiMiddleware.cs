using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Shared.Failures.Exceptions.WebApi.ClientErrors;
using Shared.Failures.Exceptions.WebApi.ServerErrors;
using System.Net.Sockets;

namespace Shared.AspNet.CustomMiddlewares.Exceptions
{
    public class DbExceptionsToWebApiMiddleware : CustomMiddleware
    {
        public override async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (DbUpdateException ex)
            {
                await HandleDbUpdateException(ex, context);
            }
            catch (NpgsqlException ex)
                when (ex.InnerException.GetType() == typeof(SocketException))
            {
                throw new ServiceUnavailableException(ex.Message);
            }
        }

        private async Task HandleDbUpdateException(
            DbUpdateException ex,
            HttpContext httpContext)
        {
            var inner = ex.InnerException;

            if (inner is NpgsqlException npgSqlEx)
            {
                switch (npgSqlEx.SqlState)
                {
                    case PostgresErrorCodes.UniqueViolation:
                        throw new ConflictException(npgSqlEx.Message);
                }
            }
        }

        public DbExceptionsToWebApiMiddleware(RequestDelegate next) : base(next)
        {
        }
    }
}
