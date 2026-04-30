using EventManager.Domain.Events;
using EventManager.DTOs.Events;

namespace EventManager.Services.Events
{
    public interface IEventsRepository
    {
        Task<Guid> AddNewAsync(
            NewEventDto eventDto,
            CancellationToken cancellationToken);

        Task<Event> GetByIdAsync(
            Guid id, 
            CancellationToken cancellationToken);

        Task DeleteAsync(
            Guid id, 
            CancellationToken cancellationToken);

        Task<IReadOnlyCollection<Event>> GetAllAsync(
            GetEventsWithFiltersDto eventsDto, 
            CancellationToken cancellationToken);

        Task CompleteUpdateAsync(
            Guid id,
            PutEventDto putEvent,
            CancellationToken cancellationToken);
    }
}
