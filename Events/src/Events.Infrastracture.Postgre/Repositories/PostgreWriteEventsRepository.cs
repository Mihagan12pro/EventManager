using Events.Application.Dtos;
using Events.Application.Repositories;
using Events.Domain;
using Events.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

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

        public async Task DeleteAsync(
            Guid id, 
            CancellationToken cancellationToken)
        {
            Event? @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (@event != null)
            {
                _dbContext.Events.Remove(@event);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateAsync(
            Guid id, 
            UpdateEventDto updateEvent,
            CancellationToken cancellationToken)
        {
            Event? @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (@event != null)
            {
                @event.EventNaming = @event.EventNaming.Update(
                    updateEvent.Title, 
                    updateEvent.Description);
            }
        }

        public PostgreWriteEventsRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
