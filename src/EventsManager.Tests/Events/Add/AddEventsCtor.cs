using EventManager.Services.Events;

namespace EventsManager.Tests.Events.Add
{
    [Collection("Events collection")]
    public partial class AddEventsTests
    {
        private readonly IEventsService _eventsService;
        private readonly EventsSeeder _eventsSeeder;

        public AddEventsTests(EventsSeeder seeder)
        {
            _eventsSeeder = seeder;
            _eventsService = seeder.EventsService;
        }
    }
}
