using Events.Application.Dtos;
using Events.Domain;

namespace Events.Application.Repositories
{
    public interface IWriteEventsRepository
    {
        Task<Guid> AddAsync(
            Event @event, 
            CancellationToken cancellationToken);
    }
}
