using EventManager.Domain.ValueObjects.DateAndTime;
using EventManager.DTOs.Shared;

namespace EventManager.DTOs.Events
{
    public record GetEventsWithFiltersDto(
        string? Title,
        PaginationDto Pagination,
        DateTimeRange? DateRange);
}
