using EventManager.Domain.Entities.Bookings;
using EventManager.DTOs.Bookings;
using EventManager.Shared.Filters;

namespace EventManager.Application.Repositories
{
    public interface IBookingsRepository
    {
        Task<Guid> CreateNewBookingAsync(
            Guid eventId,
            CancellationToken cancellationToken);

        Task ProcessBookingAsync(
            BookingProcessedDto bookingProcessedDto,
            CancellationToken cancellationToken);

        Task<BookingEntity> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<IEnumerable<BookingEntity>> GetAllAsync(
            Filters<BookingEntity> filters,
            CancellationToken cancellationToken);
    }
}
