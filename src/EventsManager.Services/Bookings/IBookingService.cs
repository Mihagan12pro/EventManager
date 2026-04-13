using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;

namespace EventManager.Services.Bookings
{
    public interface IBookingsService
    {
        Task<BookingAcceptedDto> CreateBookingAsync(Guid eventId);

        Task<Booking> GetBookingByIdAsync(Guid bookingId);

        Task<IEnumerable<Booking>> GetAllAsync(BookingFiltersDto filtersDto);

        Task<IEnumerable<Booking>> GetPendingAsync();
    }
}
