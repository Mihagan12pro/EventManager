using EventManager.Services.Events;

namespace EventsManager.Tests.Events.Put
{
    [Collection("Events collection")]
    public partial class PutEventsTests
    {
        private readonly IEventsService _eventsService;
        private readonly EventsSeeder _eventsSeeder;

        public PutEventsTests(EventsSeeder eventsSeeder)
        {
            _eventsSeeder = eventsSeeder;
            _eventsService = eventsSeeder.EventsService;
        }
    }
}
