using Events.Application.Dtos;
using Events.Application.Repositories.Events;
using Events.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Objects.Classes.Collections;
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
            Event @event = await _dbContext.Events.FirstAsync(e => e.Id == eventId, token);

            return @event;
        }

        public async Task<PaginatedEventsDto> GetPaginatedEventsAsync(
            Filters<Event> filters,
            Pagination pagination,
            CancellationToken token)
        {
            IQueryable<Event> events = _dbContext.Events;

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

        public async Task<IEnumerable<Event>> GetMostPopularAsync(
            int count, 
            CancellationToken token)
        {
            var events = _dbContext.Events.OrderByDescending(e =>
                (double)(e.TotalSeats - e.AvailableSeats) / e.TotalSeats)
                    .Take(count);

            return events;
        }

        public PostgreReadEventsRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
