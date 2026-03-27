using EventManager.Domain.Bookings;

namespace EventManager.Services.Bookings
{
    public interface IBookingService
    {
        Task<Guid> CreateBookingAsync(Guid eventId);

        Task<Booking> GetBookingByIdAsync(Guid bookingId);
    }
}
