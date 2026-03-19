using EventManager.Services.Events;

namespace EventsManager.Tests.Events.Add
{
    public partial class AddEventsTests
    {
        private readonly IEventsService _eventsService;

        public AddEventsTests()
        {
            _eventsService = new EventsService();
        }
    }
}
