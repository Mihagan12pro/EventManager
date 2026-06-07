using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;

namespace EventManager.Handlers.Events.GetEvents
{
    public record GetEventsCommand(
        GetEventsWithFiltersDto EventsFiltersDto) : ICommand;
}
