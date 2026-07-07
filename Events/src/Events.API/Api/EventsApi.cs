using Events.Application.Dtos;
using Events.Application.Handlers.Add;
using Events.Application.Handlers.GetByIdEvent;
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
            var apiGroup = app.MapGroup("api/events");

            apiGroup.MapPost("", async (
                NewEventDto @event,
                ClaimsPrincipal user,
                [FromServices] ICommandHandler<Guid, AddEventCommand> handler,
                CancellationToken token) =>
            {
                Guid id = await handler.HandleAsync(new AddEventCommand(@event), token);


                return Results.Ok(id);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

            apiGroup.MapGet("{id}", async (
                [FromRoute] Guid id,
                [FromServices]ICommandHandler < GetEventDto, GetByIdEventCommand> handler,
                CancellationToken token) => 
            {
                var @event = await handler.HandleAsync(new GetByIdEventCommand(id), token);

                return @event;

            }).RequireAuthorization();


           return app;
        }
    }
}
