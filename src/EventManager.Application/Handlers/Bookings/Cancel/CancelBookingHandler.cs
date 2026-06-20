using EventManager.Application.Repositories;
using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.DTOs.Bookings;

namespace EventManager.Application.Handlers.Bookings.Cancel
{
    public class CancelBookingHandler : ICommandHandler<CancelBookingCommand>
    {
        private readonly IBookingsRepository _bookingsRepository;

        public async Task HandleAsync(
            CancelBookingCommand command,
            CancellationToken cancellationToken)
                => await _bookingsRepository.ProcessBookingAsync(new BookingProcessedDto(command.BookingId, BookingStatus.Cancelled), cancellationToken);

        public CancelBookingHandler(IBookingsRepository bookingsRepository)
        {
            _bookingsRepository = bookingsRepository;
        }
    }
}
