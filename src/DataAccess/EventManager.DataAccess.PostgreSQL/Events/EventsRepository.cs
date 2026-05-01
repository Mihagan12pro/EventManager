using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.Services.Events;
using Microsoft.EntityFrameworkCore;

namespace EventManager.DataAccess.PostgreSQL.Events
{
    internal class EventsRepository : IEventsRepository
    {
        private readonly AppDbContext _dbContext;


        public EventsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> AddNewAsync(
            NewEventDto eventDto,
            CancellationToken cancellationToken)
        {
            Event @event = new Event()
            {
                Title = eventDto.Title,

                StartAt = eventDto.StartAt!.Value,

                EndAt = eventDto.EndAt!.Value,

                TotalSeats = eventDto.TotalSeats!.Value,

                AvailableSeats = eventDto.TotalSeats.Value,

                Description = eventDto.Description
            };

            await _dbContext.Events.AddAsync(@event, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return @event.Id;
        }

        public async Task CompleteUpdateAsync(
            Guid id,
            PutEventDto putEvent,
            CancellationToken cancellationToken)
        {
            Event @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            @event.StartAt = putEvent.StartAt.Value;
            @event.EndAt = putEvent.EndAt.Value;
            @event.Title = putEvent.Title;
            @event.Description = putEvent.Description;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(
            Guid id, 
            CancellationToken cancellationToken)
        {
            Event @event = await GetByIdAsync(id, cancellationToken);

            _dbContext.Events.Remove(@event);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<Event>> GetEventsAsync(GetEventsWithFiltersDto eventsDto, CancellationToken cancellationToken)
        {
            IQueryable<Event> events = _dbContext.Events;

            if (eventsDto.Title != null)
            {
                events = events.Where(e => e.Title.Contains(eventsDto.Title));
            }
            if (eventsDto.DateRange.LowerBound.HasValue)
            {
                events = events.Where(e => e.StartAt == eventsDto.DateRange.LowerBound.Value);
            }
            if (eventsDto.DateRange.UpperBound.HasValue)
            {
                events = events.Where(e => e.EndAt == eventsDto.DateRange.UpperBound.Value);
            }

            events = events.Skip(eventsDto.Pagination.Page)
                .Take(eventsDto.Pagination.PageSize);

            return (await events.ToListAsync(cancellationToken))
                .AsReadOnly();
        }

        public async Task<Event> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Event @event = await _dbContext.Events.FirstOrDefaultAsync((e => e.Id == id), cancellationToken);

            return @event;
        }
    }
}
