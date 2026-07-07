using Events.Application.Dtos;
using Events.Domain;
using Shared.Objects.Classes.Collections;
using Shared.Objects.Records;

namespace Events.Application.Repositories
{
    public interface IReadEventsRepository
    {
        Task<Event> GetEventAsync(
            Guid eventId, 
            CancellationToken token);

        Task<PaginatedEventsDto> GetPaginatedEventsAsync(
            Filters<Event> filters,
            Pagination pagination,
            CancellationToken token);
    }
}
