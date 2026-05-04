using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;

namespace EventManager.Services.Bookings
{
    public interface IBookingsService
    {
        Task<BookingAcceptedDto> CreateBookingAsync(
            Guid eventId, 
            CancellationToken cancellationToken);

        Task<GetBookingDto> GetBookingByIdAsync(
            Guid bookingId, 
            CancellationToken cancellationToken);

        Task<IEnumerable<GetBookingDto>> GetAllAsync(
            BookingFiltersDto filtersDto, 
            CancellationToken cancellationToken);
    }
}
