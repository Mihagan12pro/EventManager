using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;
using EventManager.Repositories.Bookings;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using EventsManager.Shared;

namespace EventManager.Handlers.Bookings.GetByIdBooking
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
