using Events.Domain;

namespace Events.Application.Repositories
{
    public interface IReadEventsRepository
    {
        Task<Event> GetEventAsync(
            Guid eventId, 
            CancellationToken token);
    }
}
