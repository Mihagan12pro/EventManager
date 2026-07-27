using Events.Application.Dtos;
using Events.Application.Repositories.Events;
using Events.Domain;
using Events.Domain.ValueObjects;
using Events.Infrastracture.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Events.Infrastracture.Repositories.Events
{
    internal class PostgreWriteEventsRepository : IWriteEventsRepository
    {
        private readonly ILogger<PostgreWriteEventsRepository> _logger;

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

                _logger.LogInformation("The event with id = {id} had been deleted", entity.Id);
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

                _logger.LogInformation("Complete update of the event with id = {id}", @event.Id);
            }
        }

        public async Task UpdateAvaliableSeats(
            Guid id,
            int avaliableSeats,
            CancellationToken cancellationToken)
        {
            EventEntity entity = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (entity != null)
            {
                int oldAvaviableSeats = entity.AvailableSeats;

                Event @event = EventEntity.ExtractEvent(entity);

                Seats seats = new Seats(@event.TotalSeats, avaliableSeats);

                @event.Seats = seats;

                @event.Validate();

                entity.Update(@event);

                int currentAvaliableSeats = entity.AvailableSeats;

                await _dbContext.SaveChangesAsync(cancellationToken);

                if (oldAvaviableSeats > currentAvaliableSeats)
                {
                    _logger.LogInformation(
                        "The number of avaliable seats for the event with id = {id} had been decreased from {oldAvaviableSeat} to {currentAvaliableSeats}",
                        @event.Id, 
                        oldAvaviableSeats, 
                        currentAvaliableSeats
                   );

                    return;
                }

                _logger.LogInformation(
                    "The number of avaliable seats for the event with id = {id}  had been increased from {oldAvaviableSeat} to {currentAvaliableSeats}", 
                    @event.Id, 
                    oldAvaviableSeats,
                    currentAvaliableSeats
                );
            }
        }

        public PostgreWriteEventsRepository(
            EventsDbContext dbContext,
            ILogger<PostgreWriteEventsRepository> logger)
        {
            _dbContext = dbContext;

            _logger = logger;
        }
    }
}
