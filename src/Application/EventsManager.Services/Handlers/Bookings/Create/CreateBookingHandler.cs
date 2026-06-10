using EventManager.Application.Repositories;
using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;

namespace EventManager.Application.Handlers.Bookings.Create
{
    public class CreateBookingHandler : ICommandHandler<BookingAcceptedDto, CreateBookingCommand>
    {
        private readonly IBookingsRepository _bookingsRepository;
        //private readonly IEventsRepository _eventsRepository;

        public async Task<BookingAcceptedDto> HandleAsync(
            CreateBookingCommand command, 
            CancellationToken cancellationToken)
        {
            Guid eventId = command.EventId;

            Guid? bookingId;
            BookingAcceptedDto? result = null;

            //EventEntity @event = await _eventsRepository.GetByIdAsync(eventId, cancellationToken);
            //NullChecker.Check(@event);

            //if (@event.AvailableSeats == 0)
            //    throw new NoAvailableSeatsException();

            Guid id = await _bookingsRepository.CreateNewBookingAsync(eventId, cancellationToken);

            result = new BookingAcceptedDto(
                id,
                eventId,
                BookingStatus.Pending);

            return result;
        }

        public CreateBookingHandler(
            IBookingsRepository bookingsRepository
/*          ,IEventsRepository eventsRepository*/)
        {
            _bookingsRepository = bookingsRepository;
           // _eventsRepository = eventsRepository;
        }
    }
}
