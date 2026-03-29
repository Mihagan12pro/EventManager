namespace EventManager.Queues.ApplicationTasks.Booking
{
    public class BookingTask : ApplicationTask
    {
        public readonly Guid Id;

        public BookingTask(Guid id)
        {
            Id = id;
        }
    }
}
