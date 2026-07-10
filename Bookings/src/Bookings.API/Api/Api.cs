using Bookings.Application.Handlers.Create;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Shared.AspNet.Utils;
using Shared.Objects.Interfaces;

namespace Bookings.API.Api
{
    internal static class Api
    {
        public static WebApplication AddApi(this WebApplication app)
        {
            app.MapPost(@"api/events/{id}/book", async(
                Guid id,
                HttpContext context,
                [FromServices] ICommandHandler<Guid, CreateBookingCommand> handler,
                CancellationToken token) => 
            {
            
                Guid result = await handler.HandleAsync(new CreateBookingCommand(id), token);

                var location = UrlMaster.CreateWithoutPath(context.Request, "api/bookings", result);

                return Results.Accepted(location, result);

            }).RequireAuthorization();

            return app;
        }
    }
}
