using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;
using System.Linq.Expressions;

namespace EventManager.Repositories.Bookings
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

        Task<IEnumerable<BookingModel>> GetAllAsync(
            Expression<Func<BookingModel, bool>> filters,
            CancellationToken cancellationToken);
    }
}
