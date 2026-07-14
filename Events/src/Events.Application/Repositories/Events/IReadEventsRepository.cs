using Events.Application.Dtos;
using Events.Domain;
using Shared.Objects.Classes.Collections;
using Shared.Objects.Records;

namespace Events.Application.Repositories.Events
{
    public interface IReadEventsRepository
    {
        Task<Event> GetEventAsync(
            Guid id,
            CancellationToken token);

        Task<PaginatedEventsDto> GetPaginatedEventsAsync(
            Filters<Event> filters,
            Pagination pagination,
            CancellationToken token);
    }
}
