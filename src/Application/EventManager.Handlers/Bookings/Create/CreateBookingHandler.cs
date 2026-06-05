using EventManager.Domain.Bookings.Enums;
using EventManager.Domain.Events;
using EventManager.DTOs.Bookings;
using EventManager.Repositories.Bookings;
using EventManager.Repositories.Events;
using EventManager.Services.Exceptions.WebApi.Client.Conflict;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;

namespace EventManager.Handlers.Bookings.Create
{
    public class CreateBookingHandler : ICommandHandler<BookingAcceptedDto, CreateBookingsCommand>
    {
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IEventsRepository _eventsRepository;

        public async Task<BookingAcceptedDto> HandleAsync(
            CreateBookingsCommand command, 
            CancellationToken cancellationToken)
        {
            Guid eventId = command.EventId;

            Guid? bookingId;
            BookingAcceptedDto? result = null;

            EventModel @event = await _eventsRepository.GetByIdAsync(eventId, cancellationToken);
            if (@event == null)
                throw new NotFoundException($"Event with id = {eventId} does not exists!");

            if (@event.AvailableSeats == 0)
                throw new NoAvailableSeatsException();

            Guid id = await _bookingsRepository.CreateNewBookingAsync(eventId, cancellationToken);

            result = new BookingAcceptedDto(
                id,
                eventId,
                BookingStatus.Pending);

            return result;
        }

        public CreateBookingHandler(
            IBookingsRepository bookingsRepository, 
            IEventsRepository eventsRepository)
        {
            _bookingsRepository = bookingsRepository;
            _eventsRepository = eventsRepository;
        }
    }
}
