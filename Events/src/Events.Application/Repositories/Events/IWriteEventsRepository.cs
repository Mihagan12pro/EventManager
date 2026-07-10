using Events.Application.Dtos;
using Events.Domain;

namespace Events.Application.Repositories.Events
{
    public interface IWriteEventsRepository
    {
        Task<Guid> AddAsync(
            Event @event, 
            CancellationToken cancellationToken);

        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task UpdateAsync(
            Guid id,
            UpdateEventDto updateEvent,
            CancellationToken cancellationToken);
    }
}
