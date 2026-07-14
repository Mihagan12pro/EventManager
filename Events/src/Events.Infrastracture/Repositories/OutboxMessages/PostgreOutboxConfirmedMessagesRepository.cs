using Events.Application.Repositories.OutboxMessages;
using Shared.Messaging.Contracts.Bookings;

namespace Events.Infrastracture.Repositories.OutboxMessages
{
    internal class PostgreOutboxConfirmedMessagesRepository : IOutboxConfirmedMessagesRepository
    {
        private readonly EventsDbContext _dbContext;

        public async Task AddAsync(
            ConfirmedBooking confirmedBooking,
            CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(confirmedBooking, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetActiveCountAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;

            var eventBooking = _dbContext.OutboxConfirmedBookingsMessages
                .Where(cbm => cbm.UserId == userId)
                .Join(_dbContext.Events,
                    cbm => cbm.EventId,

                    e => e.Id,

                    (cbm, e) => new
                    {
                        StartAt = e.StartAt
                    }
                ).Where(r => r.StartAt > now);

            return eventBooking.Count();
        }

        public PostgreOutboxConfirmedMessagesRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
