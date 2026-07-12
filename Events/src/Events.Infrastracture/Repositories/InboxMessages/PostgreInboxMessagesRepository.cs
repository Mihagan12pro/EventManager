using Events.Application.Repositories.InboxMessages;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging.Contracts.Bookings;

namespace Events.Infrastracture.Repositories.InboxMessages
{
    internal class PostgreInboxMessagesRepository : IInboxMessagesRepository
    {
        private readonly EventsDbContext _dbContext;

        public async Task AddMessageAsync(
            PendingBooking message,
            CancellationToken cancellationToken)
        {
            await _dbContext.InboxPendingMessages.AddAsync(message, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> FindPendingMessageAsync(
            Guid messageId,
            CancellationToken cancellationToken)
        {
            var message = await _dbContext.InboxPendingMessages.FirstOrDefaultAsync(
                m => m.BookingId == messageId, cancellationToken);

            return message != null;
        }

        public PostgreInboxMessagesRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
