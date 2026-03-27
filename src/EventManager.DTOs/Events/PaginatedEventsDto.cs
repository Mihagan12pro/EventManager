using EventManager.DomainModels.Events;

namespace EventManager.DTOs.Events
{
    public record PaginatedEventsDto(
        int TotalCount,
        IReadOnlyList<Event> Events,
        int Page, 
        int PageSize
    );
}
