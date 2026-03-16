using CSharpFunctionalExtensions;
using EventManager.DomainModels.Events;
using EventManager.DTOs.Events;
using EventManager.Shared;

namespace EventManager.Services.Events
{
    public class EventsService : IEventsService
    {
        private static readonly List<Event> _events = new List<Event>();

        public async Task<Result<Guid, string>> AddNew(NewEventDto request)
        {
            if (request.StartAt >= request.EndAt)
                return "End time must be greater than start time!";

            Event createdEvent = new Event()
            {
                Id = Guid.NewGuid(),

                Title = request.Title,

                StartAt = request.StartAt!.Value,

                EndAt = request.EndAt!.Value,

                Description = request.Description
            };

            _events.Add(createdEvent);

            return createdEvent.Id;
        }

        public async Task<Result<string, Error>> Delete(Guid id)
        {
            Event? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
                return Error.CreateError404($"Event with id = '{id}' was not found!");

            _events.Remove(eventById);

            return "Event had been deleted!";
        }

        public async Task<Result<GetEventDto, string>> GetEventById(Guid id)
        {
            Event? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
            {
                return $"Event with id = '{id}' was not found!";
            }

            GetEventDto eventDto = new GetEventDto(
                eventById.Title,
                eventById.StartAt, 
                eventById.EndAt,
                eventById.Description);

            return eventDto;
        }

        public async Task<IEnumerable<Event>> GetEvents(string? title, DateRange dateRange)
        {
            var filteredEvents = _events
                .Where(e => dateRange
                    .CheckDateRange(e)
                        .IsSuccess);

            if (title != null)
                filteredEvents = filteredEvents.Where(e => e.Title.ToLower() == title.ToLower());

            return filteredEvents
                .ToArray()
                    .AsReadOnly();
        }

        public async Task<Result<string, Error>> UpdateByPut(Guid id, NewEventDto putEvent)
        {
            Event? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
                return Error.CreateError404($"Event with id = '{id}' was not found!");

            if (putEvent.StartAt >= putEvent.EndAt)
                return Error.CreateError500("End time must be greater than start time!");

            eventById.StartAt = putEvent.StartAt!.Value;
            eventById.EndAt = putEvent.EndAt!.Value;
            eventById.Title = putEvent.Title;
            eventById.Description = putEvent.Description;

            return "Event had been updated!";
        }
    }
}
