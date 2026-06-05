using EventManager.DTOs.Bookings;
using EventManager.Repositories.Bookings;

namespace EventManager.Handlers.Bookings.GetAllBookings
{
    public class GetAllBookingsHandler : ICommandHandler<IEnumerable<GetBookingDto>, GetAllBookingsCommand>
    {
        private readonly IBookingsRepository _bookingRepository;

        public async Task<IEnumerable<GetBookingDto>> HandleAsync(
            GetAllBookingsCommand command,
            CancellationToken cancellationToken)
        {
            var filtersDto = command.FiltersDto;

            var result = await _bookingRepository.GetAllAsync(filtersDto, cancellationToken);

            return result.Select(b => new GetBookingDto(
                b.EventId,
                b.CreatedAt,
                b.ProcessedAt,
                b.Status));
        }

        public GetAllBookingsHandler(IBookingsRepository bookingsRepository)
        {
            _bookingRepository = bookingsRepository;
        }
    }
}
