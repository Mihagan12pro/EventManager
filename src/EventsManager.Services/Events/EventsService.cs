using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using Shared;

namespace EventManager.Services.Events
{
    internal class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;

        public async Task<Guid> AddNewAsync(NewEventDto request, CancellationToken cancellationToken)
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

            Guid id = await _eventsRepository.AddNewAsync(request, cancellationToken);

            return id;
        }

        public async Task<string> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            Event? @event = await _eventsRepository.GetByIdAsync(id, cancellationToken);

            if (@event == null)
                throw new NotFoundException($"Event with id = {id} does not exists!");

            await _eventsRepository.DeleteAsync(id, cancellationToken);

            return $"Event with id = {id} had been deleted!";
        }

        public async Task<Event> GetEventByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Event? @event = await _eventsRepository.GetByIdAsync(id, cancellationToken);

            if (@event == null)
                throw new NotFoundException($"Event with id = {id} does not exists!");
            return @event;
        }

        public async Task<PaginatedEventsDto> GetEventsAsync(
            string? title,
            PaginationDto pagination,
            DateRange dateRange, 
            CancellationToken cancellationToken)
        {
            if (pagination.Page < 0 || pagination.PageSize < 0)
                throw new BadRequestException("Pagination parameters must be greater than zero!");

            GetEventsWithFiltersDto eventsWithFiltersDto = new GetEventsWithFiltersDto(title, pagination, dateRange);

            var events = await _eventsRepository.GetEventsAsync(eventsWithFiltersDto, cancellationToken);

            return new PaginatedEventsDto(events.Count, events, pagination.Page, pagination.PageSize);
        }

        public async Task<string> UpdateByPutAsync(
            Guid id,
            PutEventDto putEvent,
            CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;

            DateTime start = putEvent.StartAt!.Value;
            DateTime end = putEvent.EndAt!.Value;

            DateSpan startSpan = new DateSpan(start, now);
            DateSpan endSpan = new DateSpan(end, now);

            Event? eventById = await _eventsRepository.GetByIdAsync(id, cancellationToken);

            if (eventById == null)
                throw new NotFoundException($"Event with id = '{id}' was not found!");

            if (startSpan.Day <= 0 && startSpan.Year <= 0 && startSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (endSpan.Day <= 0 && endSpan.Year <= 0 && endSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (putEvent.StartAt >= putEvent.EndAt)
                throw new BadRequestException("End time must be greater than start time!");

            await _eventsRepository.CompleteUpdateAsync(id, putEvent, cancellationToken);

            return $"Event with id = {id} had been updated!";
        }


        public EventsService(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }
    }
}
