using EventManager.Services.Bookings;
using System.Reflection;

namespace EventsManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
        private readonly Type _bookingServiceType;

        public CreateBookTests()
        {
            _bookingServiceType = Type.GetType("EventManager.Services.Events.EventsService, EventManager.Services");

            Type errorType = Type.GetType("Shared.Error, Shared");
        }
    }
}
