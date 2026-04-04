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

    class MockBookingTaskQueue : IBookingTaskQueue
    {
        public void Enqueue(BookingTask task)
        {
            throw new NotImplementedException();
        }

        public bool TryDequeue(out BookingTask task)
        {
            throw new NotImplementedException();
        }
    }
}
