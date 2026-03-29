using EventManager.Queues.ApplicationTasks.Booking;
using System.Collections.Concurrent;

namespace EventManager.Queues.Queues.Booking
{
    internal class InMemoryBookingTaskQueue : IBookingTaskQueue
    {
        private readonly ConcurrentQueue<BookingTask> _taskQueue = new ConcurrentQueue<BookingTask>();

        public void Enqueue(BookingTask task)
        {
            _taskQueue.Enqueue(task);
        }

        public bool TryDequeue(out BookingTask task)
        {
            return _taskQueue.TryDequeue(out task);
        }
    }
}
