using EventManager.Application.Repositories;
using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;
using EventManager.Shared;

namespace EventManager.Application.Handlers.Bookings.GetByIdBooking
{
    public class GetByIdBookingHandler : ICommandHandler<GetBookingDto, GetByIdBookingCommand>
    {
        private readonly IBookingsRepository _bookingsRepository;

        public async Task<GetBookingDto> HandleAsync(
            GetByIdBookingCommand command, 
            CancellationToken cancellationToken)
        {
            Guid bookingId = command.BookingId;

            BookingModel booking = await _bookingsRepository.GetByIdAsync(bookingId, cancellationToken);
            NullChecker.Check(booking);

            return new GetBookingDto(
                booking.EventId,
                booking.CreatedAt,
                booking.ProcessedAt,
                booking.Status);
        }

        public GetByIdBookingHandler(IBookingsRepository bookingsRepository)
        {
            _bookingsRepository = bookingsRepository;
        }
    }
}
