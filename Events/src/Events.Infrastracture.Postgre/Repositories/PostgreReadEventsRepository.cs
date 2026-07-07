using Events.Application.Repositories;
using Events.Domain;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastracture.Postgre.Repositories
{
    internal class PostgreReadEventsRepository : IReadEventsRepository
    {
        private readonly EventsDbContext _dbContext;

        public async Task<Event> GetEventAsync(
            Guid eventId,
            CancellationToken token)
        {
            Event @event = await _dbContext.Events.FirstAsync(e => e.Id == eventId, token);

            return @event;
        }

        public PostgreReadEventsRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
