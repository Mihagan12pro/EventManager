using EventManager.Domain.Bookings;
using System.Collections.Concurrent;

namespace EventManager.TasksManagers.Bookings
{
    internal class InMemoryBookingTaskQueue : IBookingTaskQueue
    {
        private readonly ConcurrentQueue<Booking> _queue = new ConcurrentQueue<Booking>();

        public void Enqueue(Booking task)
        {
            _queue.Enqueue(task);
        }

        public bool TryDequeue(out Booking task)
        {
            return _queue.TryDequeue(out task);
        }
    }
}
