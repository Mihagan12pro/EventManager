namespace EventManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
        private readonly Type _bookingHandlingServiceType;
        private readonly Type _bookingsServiceType;
        private readonly Type _eventsServiceType;

        public CreateBookTests()
        {
            _bookingHandlingServiceType = Type.GetType("EventManager.Services.Background.Bookings.BookingHandlingService, EventManager.Services");  

            _eventsServiceType = Type.GetType("EventManager.Services.Events.EventsService, EventManager.Services");
            _bookingsServiceType = Type.GetType("EventManager.Services.Bookings.BookingsService, EventManager.Services");
        }
    }
}
