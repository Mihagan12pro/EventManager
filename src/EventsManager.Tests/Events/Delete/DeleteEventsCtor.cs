using EventManager.Services.Events;

namespace EventsManager.Tests.Events.Delete
{
    public partial class DeleteEventsTests : IAsyncLifetime
    {
        private readonly IEventsService _eventsService;
        private readonly EventsSeeder _eventsSeeder;

        public DeleteEventsTests()
        {
            _eventsService = new EventsService();

            _eventsSeeder = new EventsSeeder(_eventsService);
        }

        public async Task InitializeAsync()
        {
            var result = await _eventsSeeder.GetSeededData();

            if (result.ToList().Count == 0)
                await _eventsSeeder.AddSeedData();
        }

        public async Task DisposeAsync()
        {
            await _eventsSeeder.DeleteSeedData();
        }
    }
}
