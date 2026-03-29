using EventManager.Domain.Bookings;

namespace EventManager.TasksManagers.Bookings
{
    public interface IBookingTaskQueue : ITaskQueue<Booking>
    {
    }
}
