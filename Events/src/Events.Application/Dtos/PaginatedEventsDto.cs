namespace Events.Application.Dtos
{
    public record PaginatedEventsDto(
        int TotalCount,
        IEnumerable<GetEventDto> Events,
        int Page,
        int PageSize
    );
}
