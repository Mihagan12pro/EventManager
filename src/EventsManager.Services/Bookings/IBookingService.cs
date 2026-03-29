using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;

namespace EventManager.Services.Bookings
{
    public interface IBookingService
    {
        Task<BookingAcceptedDto> CreateBookingAsync(Guid eventId);

        Task<Booking> GetBookingByIdAsync(Guid bookingId);
    }
}
