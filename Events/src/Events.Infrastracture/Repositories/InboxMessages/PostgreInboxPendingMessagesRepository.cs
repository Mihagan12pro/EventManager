using Events.Application.Repositories.Messages;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging.Contracts.Bookings;

namespace Events.Infrastracture.Repositories.InboxMessages
{
    internal class PostgreInboxPendingMessagesRepository : IInboxMessagesRepository<PendingBooking>
    {
        private readonly EventsDbContext _dbContext;

        public async Task AddMessageAsync(
            PendingBooking message,
            CancellationToken cancellationToken)
        {
            await _dbContext.InboxPendingMessages.AddAsync(message, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> FindMessageAsync(
            PendingBooking message,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.InboxPendingMessages.FirstOrDefaultAsync(
                m => m.BookingId == message.BookingId, cancellationToken);

            return result != null;
        }

        public PostgreInboxPendingMessagesRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
