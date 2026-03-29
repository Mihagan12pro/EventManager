using EventManager.TasksManagers.Bookings;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace EventManager.TasksManagers
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddQueues(this IServiceCollection queuesCollection)
        {
            queuesCollection.AddSingleton<IBookingTaskQueue, InMemoryBookingTaskQueue>();

            return queuesCollection;
        }
    }
}
