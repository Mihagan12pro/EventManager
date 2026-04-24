using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;

namespace EventManager.Services.Bookings
{
    internal class BookingsService : IBookingsService
    {
        public Task<BookingAcceptedDto> CreateBookingAsync(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Booking>> GetAllAsync(BookingFiltersDto filtersDto)
        {
            throw new NotImplementedException();
        }

        public Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            throw new NotImplementedException();
        }

        public Task Update(Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}
