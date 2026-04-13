using EventManager.DTOs.Events;
using EventManager.Queues.ApplicationTasks.Booking;
using EventManager.Queues.Queues.Booking;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
        
    }

    class MockBookingTaskQueue : IBookingPendingQueue
    {
        public void Enqueue(BookingPendingTask task)
        {
            throw new NotImplementedException();
        }

        public bool TryDequeue(out BookingPendingTask task)
        {
            throw new NotImplementedException();
        }
    }
}
