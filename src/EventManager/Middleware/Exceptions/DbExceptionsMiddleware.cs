using EventManager.Domain.Failures.Exceptions.WebApi.Client.Conflict;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EventManager.Middleware.Exceptions
{
    public class DbExceptionsMiddleware : CustomMiddleware
    {
        public DbExceptionsMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(DbUpdateException ex)
            {
                await HandleDbUpdateException(ex, context);
            }
        }

        private async Task HandleDbUpdateException(
            DbUpdateException ex, 
            HttpContext httpContext)
        {
            var inner = ex.InnerException;

            if (inner is NpgsqlException npgSqlEx)
            {
                switch(npgSqlEx.SqlState)
                {
                    case PostgresErrorCodes.UniqueViolation:
                        throw new ConflictException("Unique constrait violation!");
                }
            }
        }
    }
}
