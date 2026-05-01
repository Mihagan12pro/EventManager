using EventManager.Domain.Events;

namespace EventManager.DTOs.Events
{
    public record PaginatedEventsDto(
        int TotalCount,
       IReadOnlyCollection<EventModel> Events,
        int Page, 
        int PageSize
    );
}
