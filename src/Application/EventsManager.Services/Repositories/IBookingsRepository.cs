using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;
using EventsManager.Shared.Filters;

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

        Task<BookingModel> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<IEnumerable<BookingModel>> GetAllAsync(
            Filters<BookingModel> filters,
            CancellationToken cancellationToken);
    }
}
