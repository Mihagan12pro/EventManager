using Shared.Messaging;

namespace Bookings.Application.Repositories
{
    public interface IInboxMessagesRepository
    {
        Task<bool> FindMessageAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task AddMessageAsync(
            Message message, 
            CancellationToken cancellationToken);
    }
}
