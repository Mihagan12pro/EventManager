using Events.Application.Repositories.OutboxMessages;
using Microsoft.EntityFrameworkCore;
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

        public async Task DeleteAllAsync(
            Guid eventId,
            CancellationToken cancellationToken)
        {
            var confirmedBookings = _dbContext.OutboxConfirmedBookingsMessages.Where(cb => cb.EventId == eventId);

            foreach(var i in confirmedBookings)
            {
                _dbContext.OutboxConfirmedBookingsMessages.Remove(i);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(
            Guid bookingId,
            CancellationToken cancellationToken)
        {
            var booking = await _dbContext.OutboxConfirmedBookingsMessages.FirstOrDefaultAsync(
                b => b.BookingId == bookingId, cancellationToken
            );
            
            if (booking != null)
            {
                _dbContext.OutboxConfirmedBookingsMessages.Remove(booking);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public PostgreOutboxConfirmedMessagesRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
