using Events.Application.Repositories;
using Events.Domain;

namespace Events.Infrastracture.Postgre.Repositories
{
    internal class PostgreWriteEventsRepository : IWriteEventsRepository
    {
        public Task<Guid> AddAsync(
            Event @event,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
