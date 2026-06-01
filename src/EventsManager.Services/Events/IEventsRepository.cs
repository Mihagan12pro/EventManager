using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using System.Linq.Expressions;

namespace EventManager.Services.Events
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
            IEnumerable<Expression<Func<EventModel, bool>>> filters,
            PaginationDto Pagination, 
            CancellationToken cancellationToken);

        Task CompleteUpdateAsync(
            Guid id,
            PutEventDto putEvent,
            CancellationToken cancellationToken);
    }
}
