using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;

namespace EventManager.Services.Bookings
{
    internal class BookingsService : IBookingsService
    {
        public Task<BookingAcceptedDto> CreateBookingAsync(Guid eventId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Booking>> GetAllAsync(BookingFiltersDto filtersDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Booking> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Update(Booking booking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
