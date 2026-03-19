using EventManager.Services.Events;

namespace EventsManager.Tests.Events.Put
{
    [Collection("Events collection")]
    public partial class PutEventsTests : IAsyncLifetime
    {
        private readonly IEventsService _eventsService;
        private readonly EventsSeeder _eventsSeeder;

        public PutEventsTests(EventsSeeder eventsSeeder)
        {
            _eventsSeeder = eventsSeeder;
            _eventsService = eventsSeeder.EventsService;
        }

        public async Task DisposeAsync()
        {
            await _eventsSeeder.DeleteSeedData();
        }

        public async Task InitializeAsync()
        {
            await _eventsSeeder.AddSeedData();
        }
    }
}
