using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;

namespace EventManager.Services.Bookings
{
    public interface IBookingsService
    {
        Task<BookingAcceptedDto> CreateBookingAsync(Guid eventId);

        Task<Booking> GetBookingByIdAsync(Guid bookingId);

        Task<IEnumerable<Booking>> GetAllAsync(BookingFiltersDto filtersDto);

        Task Update(Booking booking);
    }
}
