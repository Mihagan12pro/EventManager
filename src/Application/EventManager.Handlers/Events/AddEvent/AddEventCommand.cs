using EventManager.DTOs.Events;

namespace EventManager.Handlers.Events.AddEvent
{
    public record AddEventCommand(NewEventDto NewEvent)
        : ICommand;
}
