using Events.Application.Repositories.InboxMessages;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Infrastracture.Repositories.InboxMessages
{
    internal class PostgreInboxMessagesRepository : InboxMessagesRepository
    {
        private readonly EventsDbContext _dbContext;

        public async Task AddMessageAsync(
            Message message,
            CancellationToken cancellationToken)
        {
            await _dbContext.InboxMessages.AddAsync(message, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> FindMessageAsync(
            Guid messageId,
            CancellationToken cancellationToken)
        {
            Message? message = await _dbContext.InboxMessages.FirstOrDefaultAsync(
                m => m.Id == messageId, cancellationToken); 

            return message != null;
        }

        public PostgreInboxMessagesRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
