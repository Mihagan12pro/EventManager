using Bookings.Application.Handlers.Create;
using Microsoft.AspNetCore.Mvc;
using Shared.Objects.Interfaces;

namespace Bookings.API.Api
{
    internal static class Api
    {
        public static WebApplication AddApi(this WebApplication app)
        {
            app.MapPost(@"api/events/{id}/book", async(
                Guid id,
                [FromServices] ICommandHandler<CreateBookingCommand> handler,
                CancellationToken token) => 
            {
            
                await handler.HandleAsync(new CreateBookingCommand(id), token);
            
            }).RequireAuthorization();

            return app;
        }
    }
}
