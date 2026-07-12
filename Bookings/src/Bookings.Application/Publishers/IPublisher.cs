using Shared.Messaging.Contracts.Bookings;

namespace Bookings.Application.Publishers
{
    public interface IPublisher
    {
        Task ProduceAsync(
            PendingBooking pendingBooking,
            CancellationToken cancellationToken);
    }
}
