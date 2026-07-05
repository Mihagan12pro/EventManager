using Events.Application.Contracts;
using Events.Application.Handlers.Add;
using Microsoft.AspNetCore.Mvc;
using Shared.Objects.Interfaces;

namespace Events.API.Api
{
    public static class EventsApi
    {
        public static WebApplication AddEventsEndPoints(this WebApplication app)
        {
            var apiGroup = app.MapGroup("events/api");

            apiGroup.MapPost("", async (
                NewEventDto @event, 
                [FromServices] ICommandHandler<Guid, AddEventCommand> handler,
                CancellationToken token) =>
            {
                Guid id = await handler.HandleAsync(new AddEventCommand(@event), token);

                return Results.Ok(id);
            });
           

            return app;
        }
    }
}
