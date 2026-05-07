using EventManager.Domain.Events;
using EventManager.DTOs.Shared;

namespace EventManager.DTOs.Events
{
    public record GetEventsWithFiltersDto(
        string? Title,
        PaginationDto Pagination,
        DateRange DateRange);
}
