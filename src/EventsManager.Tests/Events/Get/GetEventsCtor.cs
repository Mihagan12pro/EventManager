using EventManager.Services.Events;

namespace EventsManager.Tests.Events.Get
{
    [Collection("Events collection")]
    public partial class GetEventsTests 
    {
        private readonly IEventsService _eventsService;
        private readonly EventsSeeder _eventsSeeder;

        public GetEventsTests(EventsSeeder eventsSeeder)
        {
            _eventsSeeder = eventsSeeder;
            _eventsService = eventsSeeder.EventsService;
        }
    }
}
