using EventManager.DomainModels;
using EventManager.DTOs.Events;
using EventManager.Shared;

namespace EventManager.Services.Events
{
    public class EventsService : IEventsService
    {
        private static readonly List<Event> _events = new List<Event>();

        public async Task<Result> AddNew(NewEventDto request)
        {
            if (request.StartAt >= request.EndAt)
            {
                return new Result(false, "End time must be greater than start time!");
            }

            Event createdEvent = new Event() 
            {
                Id = Guid.NewGuid(),

                Title = request.Title,

                StartAt = request.StartAt,

                EndAt = request.EndAt,

                Description = request.Description
            };

            _events.Add(createdEvent);

            return new Result(true, createdEvent.Id);
        }

        public async Task<Result> Delete(Guid id)
        {
            Event? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
                return new Result(false, $"Event with id = '{id}' was not found!");

            _events.Remove(eventById);

            return new Result(true, "Event had been deleted!");
        }

        public async Task<Result> GetEventById(Guid id)
        {
            Event? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
            {
                return new Result(false, $"Event with id = '{id}' was not found!");
            }

            GetEventDto eventDto = new GetEventDto(
                eventById.Title,
                eventById.StartAt, 
                eventById.EndAt,
                eventById.Description);

            return new Result(true, eventDto);
        }

        public async Task<Result> GetEvents()
        {
            return new Result(true, _events.AsReadOnly());
        }

        public async Task<(Result, int)> UpdateByPut(Guid id, NewEventDto putEvent)
        {
            Event? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
                return (new Result(
                    false, 
                    $"Event with id = '{id}' was not found!"),
                    404);

            if (putEvent.StartAt >= putEvent.EndAt)
                return (new Result(
                    false,
                    $"End time must be greater than start time!"),
                    400);

            eventById.StartAt = putEvent.StartAt;
            eventById.EndAt = putEvent.EndAt;
            eventById.Title = putEvent.Title;
            eventById.Description = putEvent.Description;

            return (new Result(
                true,
                $"Event had been updated!"),
                200);
        }
    }
}
