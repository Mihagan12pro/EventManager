using EventManager.Queues.ApplicationTasks.Booking;

namespace EventManager.Queues.Queues.Booking
{
    public interface IBookingTaskQueue : ITaskQueue<BookingTask>
    {
    }
}
