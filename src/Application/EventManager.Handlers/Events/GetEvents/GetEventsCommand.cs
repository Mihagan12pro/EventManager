using EventManager.DTOs.Events;

namespace EventManager.Handlers.Events.GetEvents
{
    public record GetEventsCommand(
        GetEventsWithFiltersDto EventsFiltersDto) : ICommand;
}
