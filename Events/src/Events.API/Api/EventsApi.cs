using Events.Application.Dtos;
using Events.Application.Handlers.Add;
using Events.Application.Handlers.Cancel;
using Events.Application.Handlers.CompleteUpdate;
using Events.Application.Handlers.GetByIdEvent;
using Events.Application.Handlers.GetEventsCommand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Objects.Interfaces;
using Shared.Objects.Records;
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
                [FromServices]ICommandHandler <GetEventDto, GetByIdEventCommand> handler,
                CancellationToken token) => 
            {
                var @event = await handler.HandleAsync(new GetByIdEventCommand(id), token);

                return @event;

            }).RequireAuthorization();

            apiGroup.MapGet("", async (
                [FromServices]ICommandHandler <PaginatedEventsDto, GetEventsCommand> handler,
                CancellationToken token,
                [FromQuery] string? title,
                [FromQuery] DateTime? from,
                [FromQuery] DateTime? to,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10) => 
            {
                GetEventsCommand command = new GetEventsCommand(
                    title,
                    from,
                    to,
                    new Pagination(page, pageSize)
                );

                return await handler.HandleAsync(command, token);
            });

            apiGroup.MapDelete("", async (
                [FromServices] ICommandHandler <CancelEventCommand> handler,
                Guid id, 
                CancellationToken token) => 
            {
                await handler.HandleAsync(new CancelEventCommand(id), token);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

            apiGroup.MapPut("{id}", async (
                [FromServices] ICommandHandler <CompleteEventUpdateCommand> handler,
                [FromRoute] Guid id,
                [FromBody] UpdateEventDto updateEvent,
                CancellationToken token) => 
            {
                CompleteEventUpdateCommand command = new CompleteEventUpdateCommand(
                    id,
                    updateEvent
                );

                await handler.HandleAsync(command, token);

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }); ;


           return app;
        }
    }
}
