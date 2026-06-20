using EventManager.Application.Repositories;
using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.DTOs.Bookings;

namespace EventManager.Application.Handlers.Bookings.Create
{
    internal class CreateBookingHandler : ICommandHandler<BookingAcceptedDto, CreateBookingCommand>
    {
        private readonly IBookingsRepository _bookingsRepository;

        public async Task<BookingAcceptedDto> HandleAsync(
            CreateBookingCommand command, 
            CancellationToken cancellationToken)
        {
            Guid eventId = command.EventId;

            Guid? bookingId;
            BookingAcceptedDto? result = null;

            Guid id = await _bookingsRepository.CreateNewBookingAsync(eventId, cancellationToken);

            result = new BookingAcceptedDto(
                id,
                eventId,
                BookingStatus.Pending);

            return result;
        }

        public CreateBookingHandler(IBookingsRepository bookingsRepository)
        {
            _bookingsRepository = bookingsRepository;
        }
    }
}
