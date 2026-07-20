using Events.Application.Dtos;
using Events.Application.Repositories.Events;
using Events.Domain;
using Events.Domain.ValueObjects;
using Events.Infrastracture.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastracture.Repositories.Events
{
    internal class PostgreWriteEventsRepository : IWriteEventsRepository
    {
        private readonly EventsDbContext _dbContext;

        public async Task<Guid> AddAsync(
            Event @event,
            CancellationToken cancellationToken)
        {
            var entity = EventEntity.ExtractEntity(@event);

            await _dbContext.Events.AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task DeleteAsync(
            Guid id, 
            CancellationToken cancellationToken)
        {
            EventEntity entity = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (entity != null)
            {
                _dbContext.Events.Remove(entity);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateAsync(
            Guid id, 
            UpdateEventDto updateEvent,
            CancellationToken cancellationToken)
        {
            EventEntity entity = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (entity != null)
            {
                Event @event = EventEntity.ExtractEvent(entity);

                EventNaming naming = new EventNaming(
                    @event.Title,
                    @event.Description);

                EventDateTime dateTime = new EventDateTime(
                    @event.StartAt, 
                    @event.EndAt);

                @event.EventNaming = naming.Update(
                    updateEvent.Title, 
                    updateEvent.Description);

                @event.EventDateTime = dateTime.Update(
                    updateEvent.From, 
                    updateEvent.To);

                @event.Validate();

                entity.Update(@event);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public PostgreWriteEventsRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
