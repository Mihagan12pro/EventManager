using EventManager.Domain.Events;

namespace EventManager.DTOs.Events
{
    public record PaginatedEventsDto(
        int TotalCount,
        IEnumerable<GetEventDto> Events,
        int Page, 
        int PageSize
    );
}
