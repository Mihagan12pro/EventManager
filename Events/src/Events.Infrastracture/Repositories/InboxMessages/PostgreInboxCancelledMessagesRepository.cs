using Events.Application.Repositories.InboxMessages;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging.Contracts.Bookings;

namespace Events.Infrastracture.Repositories.InboxMessages
{
    internal class PostgreInboxCancelledMessagesRepository : IInboxMessagesRepository<CancelledBooking>
    {
        private readonly EventsDbContext _dbContext;

        public async Task AddMessageAsync(
            CancelledBooking message,
            CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(message, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> FindMessageAsync(
            CancelledBooking message,
            CancellationToken cancellationToken)
                => await _dbContext.InboxCancelledMessages.FirstOrDefaultAsync(
                    m => m.BookingId == message.BookingId
                   ) != null;

        public PostgreInboxCancelledMessagesRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
