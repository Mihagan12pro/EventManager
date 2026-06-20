using EventManager.Domain.Entities.Events;
using EventManager.Domain.ValueObjects;
using EventManager.DTOs.Events;
using EventManager.Shared.Filters;

namespace EventManager.Application.Repositories
{
    public interface IEventsRepository
    {
        Task<Guid> AddNewAsync(
            NewEventDto eventDto,
            CancellationToken cancellationToken);

        Task<EventEntity> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<PaginatedEventsDto> GetPaginatedEventsAsync(
            Filters<EventEntity> filters,
            Pagination Pagination,
            CancellationToken cancellationToken);

        Task CompleteUpdateAsync(
            Guid id,
            PutEventDto putEvent,
            CancellationToken cancellationToken);
    }
}
