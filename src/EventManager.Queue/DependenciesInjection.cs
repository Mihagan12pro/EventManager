using EventManager.Queues.Queues.Booking;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Queues
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddQueues(this IServiceCollection queuesCollection)
        {
            queuesCollection.AddSingleton<IBookingPendingQueue, InMemoryBookingPendingQueue>();

            return queuesCollection;
        }
    }
}
