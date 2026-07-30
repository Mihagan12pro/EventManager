using Bookings.Application.Dtos;
using Bookings.Application.Handlers.Cancel;
using Bookings.Application.Handlers.Create;
using Bookings.Application.Handlers.Get;
using Microsoft.AspNetCore.Mvc;
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

            var apiGroup = app.MapGroup("api/bookings");

            apiGroup.MapGet("{id}", async (
                [FromServices] ICommandHandler <GetBookingDto, GetByIdCommand> handler,
                Guid id, 
                CancellationToken token) => 
            {
                GetByIdCommand command = new GetByIdCommand(id);

                var reponse = await handler.HandleAsync(command, token);

                return Results.Ok(reponse);

            }).RequireAuthorization();

            apiGroup.MapDelete("{id}", async (
               [FromServices] ICommandHandler<CancelBookingCommand> handler,
               Guid id,
               CancellationToken token) =>
            {
                var command = new CancelBookingCommand(id);

                await handler.HandleAsync(command, token);

            }).RequireAuthorization();

            return app;
        }
    }
}
