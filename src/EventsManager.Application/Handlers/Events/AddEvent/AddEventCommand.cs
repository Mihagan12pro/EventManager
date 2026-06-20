using EventManager.Application;
using EventManager.DTOs.Events;

namespace EventManager.Application.Handlers.Events.AddEvent
{
    public record AddEventCommand(NewEventDto NewEvent)
        : ICommand;
}
