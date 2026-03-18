using EventManager.Services.Events;

namespace EventsManager.Tests.Events.Delete
{
    [Collection("Events collection")]
    public partial class DeleteEventsTests
    {
        private readonly IEventsService _eventsService;
        private readonly EventsSeeder _eventsSeeder;

        public DeleteEventsTests(EventsSeeder seeder)
        {
            _eventsSeeder = seeder;
            _eventsService = seeder.EventsService;
        }
    }
}
