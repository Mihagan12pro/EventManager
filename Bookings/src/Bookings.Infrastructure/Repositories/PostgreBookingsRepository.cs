using Bookings.Application.Repositories;

namespace Bookings.Infrastructure.Repositories
{
    internal class PostgreBookingsRepository : IBookingRepository
    {
        public async Task CreateAsync(
            Guid eventId,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
