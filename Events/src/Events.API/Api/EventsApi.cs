using Events.Application.Dtos;
using Events.Application.Handlers.Add;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Objects.Interfaces;
using System.Security.Claims;

namespace Events.API.Api
{
    public static class EventsApi
    {
        public static WebApplication AddEventsEndPoints(this WebApplication app)
        {
            var apiGroup = app.MapGroup("events/api");

            apiGroup.MapPost("", async (
                NewEventDto @event,
                ClaimsPrincipal user,
                [FromServices] ICommandHandler<Guid, AddEventCommand> handler,
                CancellationToken token) =>
            {
                Guid id = await handler.HandleAsync(new AddEventCommand(@event), token);


                return Results.Ok(id);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });


           return app;
        }
    }
}
