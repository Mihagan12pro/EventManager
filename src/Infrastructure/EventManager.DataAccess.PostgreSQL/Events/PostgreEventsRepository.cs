using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using EventManager.Repositories.Events;
using EventsManager.Shared.Filters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

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
            EventModel @event = new EventModel()
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
            EventModel @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

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
            EventModel @event = await GetByIdAsync(id, cancellationToken);

            _dbContext.Events.Remove(@event);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<EventModel> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            EventModel @event = await _dbContext.Events.FirstOrDefaultAsync((e => e.Id == id), cancellationToken);

            return @event;
        }

        public async Task<PaginatedEventsDto> GetPaginatedEventsAsync(
            EventsFilters filters, 
            PaginationDto pagination, 
            CancellationToken cancellationToken)
        {
            IQueryable<EventModel> events = _dbContext.Events;

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
