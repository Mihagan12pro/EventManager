using Bookings.Domain;

namespace Bookings.Application.Repositories
{
    public interface IBookingRepository
    {
        Task CreateAsync(
            Booking booking,
            CancellationToken cancellationToken);
    }
}
