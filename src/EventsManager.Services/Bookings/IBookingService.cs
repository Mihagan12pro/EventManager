using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;

namespace EventManager.Services.Bookings
{
    public interface IBookingsService
    {
        Task<BookingAcceptedDto> CreateBookingAsync(
            Guid eventId, 
            CancellationToken cancellationToken);

        Task<Booking> GetBookingByIdAsync(
            Guid bookingId, 
            CancellationToken cancellationToken);

        Task<IEnumerable<Booking>> GetAllAsync(
            BookingFiltersDto filtersDto, 
            CancellationToken cancellationToken);

        Task Update(Booking booking, 
            CancellationToken cancellationToken);
    }
}
