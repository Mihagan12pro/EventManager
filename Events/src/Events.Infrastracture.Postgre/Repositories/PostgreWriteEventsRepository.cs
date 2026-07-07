using Events.Application.Repositories;
using Events.Domain;

namespace Events.Infrastracture.Postgre.Repositories
{
    internal class PostgreWriteEventsRepository : IWriteEventsRepository
    {
        private readonly EventsDbContext _dbContext;

        public async Task<Guid> AddAsync(
            Event @event,
            CancellationToken cancellationToken)
        {
            await _dbContext.Events.AddAsync(@event, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return @event.Id;
        }

        public PostgreWriteEventsRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
