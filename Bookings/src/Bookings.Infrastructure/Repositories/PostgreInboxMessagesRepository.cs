using Bookings.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging;

namespace Bookings.Infrastructure.Repositories
{
    internal class PostgreInboxMessagesRepository : IInboxMessagesRepository
    {
        private readonly BookingsDbContext _dbContext;

        public async Task AddMessageAsync(
            Message message, 
            CancellationToken cancellationToken)
        {
            await _dbContext.InboxMessages.AddAsync(message, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> FindMessageAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            Message? message = await _dbContext.InboxMessages.FirstOrDefaultAsync(m => m.Id == id);

            return message != null;
        }

        public PostgreInboxMessagesRepository(BookingsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
