using Bookings.Domain;
using Bookings.Domain.Enums;
using Shared.Objects.Classes.Collections;

namespace Bookings.Application.Repositories
{
    public interface IBookingRepository
    {
        Task<Guid> CreateAsync(
            Booking booking,
            CancellationToken cancellationToken);

        Task<IEnumerable<Booking>> GetAllWithFiltersAsync(
            Filters<Booking> filters,
            CancellationToken cancellationToken);

        Task ChangeBookingStatusAsync(
            Guid id, 
            BookingStatus status,
            DateTime OccuredAt,
            CancellationToken cancellationToken);

        Task<Booking> GetByIdAsync(
            Guid id, 
            CancellationToken cancellationToken);
    }
}
