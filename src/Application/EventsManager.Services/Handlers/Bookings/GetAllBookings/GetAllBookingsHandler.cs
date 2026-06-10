using EventManager.Application.Repositories;
using EventManager.DTOs.Bookings;
using EventsManager.Shared.Filters;

namespace EventManager.Application.Handlers.Bookings.GetAllBookings
{
    public class GetAllBookingsHandler : ICommandHandler<IEnumerable<GetBookingDto>, GetAllBookingsCommand>
    {
        private readonly IBookingsRepository _bookingRepository;

        public async Task<IEnumerable<GetBookingDto>> HandleAsync(
            GetAllBookingsCommand command,
            CancellationToken cancellationToken)
        {
            var filters = new BookingsFilters();
            filters.Add(command.FiltersDto);

            var result = await _bookingRepository.GetAllAsync(filters, cancellationToken);

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
