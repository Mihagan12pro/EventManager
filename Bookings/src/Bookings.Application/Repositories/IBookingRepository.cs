using Bookings.Domain;
using Shared.Objects.Classes.Collections;

namespace Bookings.Application.Repositories
{
    public interface IBookingRepository
    {
        Task CreateAsync(
            Booking booking,
            CancellationToken cancellationToken);

        Task<IEnumerable<Booking>> GetAllWithFiltersAsync(
            Filters<Booking> filters,
            CancellationToken cancellationToken);
    }
}
