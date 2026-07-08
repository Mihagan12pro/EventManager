using Events.Application.Repositories;
using Events.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Messaging;
using Shared.Messaging.Contracts.Bookings;

namespace Events.Infrastracture.Consumers.Pending
{
    internal class PendingBookingsHandler : IMessageHandler<PendingBooking>
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public async Task HandleAsync(
            PendingBooking message, 
            CancellationToken cancellationToken)
        {
            await using (var  scope = _serviceScopeFactory.CreateAsyncScope())
            {
                try
                {
                    EventsDbContext dbContext = scope.ServiceProvider.GetRequiredService<EventsDbContext>();

                    Guid messageId = Guid.Parse(message.Id);

                    if (await dbContext.Outbox.FirstOrDefaultAsync(m => m.Id == messageId) == null)
                    {
                        Event @event = await dbContext.Events.FirstAsync(e => e.Id == Guid.Parse(message.EventId));
                        @event.ReverseSeats();

                        await dbContext.Outbox.AddAsync(new Message() { Id = messageId}, cancellationToken);

                        await dbContext.SaveChangesAsync(cancellationToken);
                    }

                    await _semaphore.WaitAsync();
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        public PendingBookingsHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
    }
}
