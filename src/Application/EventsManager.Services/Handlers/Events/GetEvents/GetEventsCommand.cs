using EventManager.Application;
using EventManager.DTOs.Events;

namespace EventManager.Application.Handlers.Events.GetEvents
{
    public record GetEventsCommand(
        GetEventsWithFiltersDto EventsFiltersDto) : ICommand;
}
