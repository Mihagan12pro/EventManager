using EventManager.Domain.Events;

namespace EventManager.DTOs.Events
{
    public record PaginatedEventsDto(
        int TotalCount,
        IEnumerable<EventModel> Events,
        int Page, 
        int PageSize
    );
}
