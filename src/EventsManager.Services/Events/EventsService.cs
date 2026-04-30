using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;

namespace EventManager.Services.Events
{
    internal class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;

        public Task<Guid> AddNewAsync(NewEventDto request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Event> GetEventByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedEventsDto> GetEventsAsync(string? title, PaginationDto pagination, DateRange dateRange, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateByPutAsync(Guid id, PutEventDto putEvent, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        public EventsService(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }
    }
}
