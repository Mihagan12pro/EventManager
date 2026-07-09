using Shared.Messaging.Contracts.Bookings;

namespace Bookings.Infrastructure.Messaging.Publishers
{
    internal class KafkaPublisher : IPublisher
    {
        public async Task<IPublisher> ProduceAsync(
            PendingBooking pendingBooking, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
