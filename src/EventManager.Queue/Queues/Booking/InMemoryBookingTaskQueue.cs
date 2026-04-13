using EventManager.Queues.ApplicationTasks.Booking;
using System.Collections.Concurrent;

namespace EventManager.Queues.Queues.Booking
{
    internal class InMemoryBookingPendingQueue : IBookingPendingQueue
    {
        private readonly ConcurrentQueue<BookingPendingTask> _taskQueue = new ConcurrentQueue<BookingPendingTask>();

        public void Enqueue(BookingPendingTask task)
        {
            _taskQueue.Enqueue(task);
        }

        public bool TryDequeue(out BookingPendingTask task)
        {
            return _taskQueue.TryDequeue(out task);
        }
    }
}
