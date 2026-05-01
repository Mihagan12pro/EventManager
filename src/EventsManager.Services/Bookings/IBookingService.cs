using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;

namespace EventManager.Services.Bookings
{
    public interface IBookingsService
    {
        Task<BookingAcceptedDto> CreateBookingAsync(
            Guid eventId, 
            CancellationToken cancellationToken);

        Task<BookingModel> GetBookingByIdAsync(
            Guid bookingId, 
            CancellationToken cancellationToken);

        Task<IEnumerable<BookingModel>> GetAllAsync(
            BookingFiltersDto filtersDto, 
            CancellationToken cancellationToken);
    }
}
