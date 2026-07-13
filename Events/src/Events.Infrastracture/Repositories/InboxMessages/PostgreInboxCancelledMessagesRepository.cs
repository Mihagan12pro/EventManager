using Events.Application.Repositories.InboxMessages;
using Shared.Messaging.Contracts.Bookings;

namespace Events.Infrastracture.Repositories.InboxMessages
{
    internal class PostgreInboxCancelledMessagesRepository : IInboxMessagesRepository<CancelledBooking>
    {
        public Task AddMessageAsync(CancelledBooking message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FindMessageAsync(CancelledBooking message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
