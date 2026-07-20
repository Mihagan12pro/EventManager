using Events.Application.Dtos;
using Events.Application.Repositories.Events;
using Events.Domain;
using Events.Infrastracture.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Objects.Records;

namespace Events.Infrastracture.Repositories.Events
{
    internal class PostgreReadEventsRepository : IReadEventsRepository
    {
        private readonly EventsDbContext _dbContext;

        public async Task<Event> GetEventAsync(
            Guid eventId,
            CancellationToken token)
        {
            Event @event = EventEntity.ExtractEvent(await _dbContext.Events.FirstAsync(e => e.Id == eventId, token));

            return @event;
        }

        public async Task<IEnumerable<Event>> GetMostPopularAsync(
            int count, 
            CancellationToken token)
        {
            var events = _dbContext.Events.OrderByDescending(e =>
                (double)(e.TotalSeats - e.AvailableSeats) / e.TotalSeats)
                    .Take(count);

            return events.Select(e => EventEntity.ExtractEvent(e));
        }

        public async Task<PaginatedEventsDto> GetPaginatedEventsAsync(
            string? title, 
            
            DateTime? startAt,
            
            DateTime? endAt, 
            
            Pagination pagination,
            
            CancellationToken token)
        {
            IQueryable<EventEntity> entities = _dbContext.Events;

            if (title != null)
            {
                entities = entities.Where(e => e.Title.StartsWith(title));
            }

            if (startAt != null)
            {
                entities = entities.Where(e => e.StartAt == startAt.Value);
            }

            if (endAt != null)
            {
                entities = entities.Where(e => e.EndAt == endAt.Value);
            }

            IEnumerable<GetEventDto> events = entities.Select(e => new GetEventDto(
                e.Id, 
                
                e.Title, 
                
                e.StartAt, 
                
                e.EndAt, 
                
                e.Description, 
                
                e.TotalSeats,
                
                e.AvailableSeats)
            );

            return new PaginatedEventsDto(events.Count(), events, pagination.Page, pagination.PageSize);
        }

        public PostgreReadEventsRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
