namespace EventManager.Queues.ApplicationTasks.Booking
{
    public class BookingPendingTask : ApplicationTask
    {
        public readonly Guid EventId;
        public readonly Guid Id;

        public BookingPendingTask(Guid eventId, Guid id)
        {
            Id = id;
        }
    }
}
