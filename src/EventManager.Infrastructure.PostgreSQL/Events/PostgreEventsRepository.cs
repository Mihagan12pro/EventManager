using EventManager.Application.Repositories;
using EventManager.Domain.Entities.Events;
using EventManager.Domain.ValueObjects;
using EventManager.Domain.ValueObjects.Events;
using EventManager.Domain.ValueObjects.Events.DateAndTime;
using EventManager.DTOs.Events;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using EventManager.Shared.Filters;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Infrastructure.PostgreSQL.Events
{
    public class PostgreEventsRepository : IEventsRepository
    {
        private readonly AppDbContextBase _dbContext;

        public PostgreEventsRepository(AppDbContextBase dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> AddNewAsync(
            NewEventDto eventDto,
            CancellationToken cancellationToken)
        {
            var start = eventDto.StartAt!.Value;
            var end = eventDto.EndAt!.Value;

            EventEntity @event = new EventEntity()
            {
                EventNaming =  new EventNaming(eventDto.Title, eventDto.Description),
                
                EventDateTime = new EventDateTime(start, end),
                
                Seats = new Seats(eventDto.TotalSeats!.Value)
            };
            @event.Validate();

            await _dbContext.Events.AddAsync(@event, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return @event.Id;
        }

        public async Task CompleteUpdateAsync(
            Guid id,
            PutEventDto putEvent,
            CancellationToken cancellationToken)
        {
            EventEntity @event = await _dbContext.Events.FirstAsync(e => e.Id == id, cancellationToken);
            @event.EventNaming = new EventNaming(putEvent.Title, putEvent.Description);
            @event.EventDateTime = new EventDateTime(putEvent.StartAt.Value, putEvent.EndAt.Value);
            @event.Validate();

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(
            Guid id, 
            CancellationToken cancellationToken)
        {
            EventEntity @event = await GetByIdAsync(id, cancellationToken);

            _dbContext.Events.Remove(@event);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<EventEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            EventEntity @event = await _dbContext.Events.FirstAsync((e => e.Id == id), cancellationToken);

            return @event;
        }

        public async Task<PaginatedEventsDto> GetPaginatedEventsAsync(
            Filters<EventEntity> filters, 
            Pagination pagination, 
            CancellationToken cancellationToken)
        {
            IQueryable<EventEntity> events = _dbContext.Events;

            events = filters.ApplyFilters(events);

            int count = events.Count();

            events = events.Skip(pagination.Skip)
                .Take(pagination.PageSize);

            return new PaginatedEventsDto(
                count,
                events.Select(e => new GetEventDto(
                    e.Id,
                    e.Title,
                    e.StartAt,
                    e.EndAt,
                    e.Description,
                    e.TotalSeats,
                    e.AvailableSeats)
                ),
                pagination.Page,
                pagination.PageSize);
        }
    }
}
