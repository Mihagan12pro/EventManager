using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using FluentValidation;
using Shared;

namespace EventManager.Services.Events
{
    internal class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;

        public Task<Guid> AddNewAsync(NewEventDto request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;

            if (!request.StartAt.HasValue || !request.EndAt.HasValue)
                throw new BadRequestException("Start date time and End date time are required fields!");

            DateTime start = request.StartAt.Value;
            DateTime end = request.EndAt.Value;
            int totalSeats = request.TotalSeats.Value;

            DateSpan startSpan = new DateSpan(start, now);
            DateSpan endSpan = new DateSpan(end, now);

            if (startSpan.Day <= 0 && startSpan.Year <= 0 && startSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (endSpan.Day <= 0 && endSpan.Year <= 0 && endSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (totalSeats < 1)
                throw new BadRequestException("Count of total seats must be greater than zero!");

            if (start >= end)
                throw new BadRequestException("Start date time must be greater than end date time!");


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
