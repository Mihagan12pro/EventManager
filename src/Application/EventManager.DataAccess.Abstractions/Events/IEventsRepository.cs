using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventsManager.Shared.Filters;

namespace EventManager.Repositories.Events
{
    public interface IEventsRepository
    {
        Task<Guid> AddNewAsync(
            NewEventDto eventDto,
            CancellationToken cancellationToken);

        Task<EventModel> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<PaginatedEventsDto> GetPaginatedEventsAsync(
            Filters<EventModel> filters,
            PaginationDto Pagination,
            CancellationToken cancellationToken);

        Task CompleteUpdateAsync(
            Guid id,
            PutEventDto putEvent,
            CancellationToken cancellationToken);
    }
}
