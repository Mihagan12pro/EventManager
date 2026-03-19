using EventManager.Services.Events;

namespace EventsManager.Tests.Events.Get
{
    [Collection("Events collection")]
    public partial class GetEventsTests : IAsyncLifetime
    {
        private readonly IEventsService _eventsService;
        private readonly EventsSeeder _eventsSeeder;

        public GetEventsTests(EventsSeeder eventsSeeder)
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
