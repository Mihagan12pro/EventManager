using EventManager.Domain.ValueObjects;
using EventManager.Domain.ValueObjects.Events.DateAndTime;

namespace EventManager.DTOs.Events
{
    public record GetEventsWithFiltersDto(
        string? Title,
        Pagination Pagination,
        DateTimeRange? DateRange);
}
