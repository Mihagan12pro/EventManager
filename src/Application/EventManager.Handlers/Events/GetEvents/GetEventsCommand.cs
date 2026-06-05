using EventManager.Domain.Events;
using EventManager.DTOs.Shared;

namespace EventManager.Handlers.Events.GetEvents
{
    public record GetEventsCommand(
        string? Title,
        PaginationDto Pagination,
        DateRange DateRange) : ICommand;
}
