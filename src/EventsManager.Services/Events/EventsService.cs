using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;

namespace EventManager.Services.Events
{
    internal class EventsService : IEventsService
    {
        public Task<Guid> AddNewAsync(NewEventDto request)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Event> GetEventByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedEventsDto> GetEventsAsync(string? title, PaginationDto pagination, DateRange dateRange)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateByPutAsync(Guid id, NewEventDto putEvent)
        {
            throw new NotImplementedException();
        }
    }
}
