using EventManager.Application.Repositories;
using EventManager.Domain.Entities.Bookings;
using EventManager.DTOs.Bookings;
using EventManager.Shared;

namespace EventManager.Application.Handlers.Bookings.GetByIdBooking
{
    internal class GetByIdBookingHandler : ICommandHandler<GetBookingDto, GetByIdBookingCommand>
    {
        private readonly IBookingsRepository _bookingsRepository;

        public async Task<GetBookingDto> HandleAsync(
            GetByIdBookingCommand command, 
            CancellationToken cancellationToken)
        {
            Guid bookingId = command.BookingId;

            BookingEntity booking = await _bookingsRepository.GetByIdAsync(bookingId, cancellationToken);
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
