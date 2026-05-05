using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;

namespace EventManager.Services.Bookings
{
    public interface IBookingsRepository
    {
        Task<Guid> CreateNewBookingAsync(
            Guid eventId, 
            CancellationToken cancellationToken);

        Task ProcessBookingAsync(
            BookingProcessedDto bookingProcessedDto, 
            CancellationToken cancellationToken);

        Task<BookingModel> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<IEnumerable<BookingModel>> GetAllAsync(
            BookingFiltersDto bookingFiltersDto, 
            CancellationToken cancellationToken);
    }
}
