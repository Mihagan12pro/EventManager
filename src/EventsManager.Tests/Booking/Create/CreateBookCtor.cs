namespace EventsManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
        private readonly Type _bookingsServiceType;
        private readonly Type _eventsServiceType;

        public CreateBookTests()
        {
            _eventsServiceType = Type.GetType("EventManager.Services.Events.EventsService, EventManager.Services");
            _bookingsServiceType = Type.GetType("EventManager.Services.Bookings.BookingsService, EventManager.Services");
        }
    }
}
