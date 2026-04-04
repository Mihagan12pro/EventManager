namespace EventManager.Tests.Events.Get
{
    public partial class GetEventsTests : EventsTests
    {
        private readonly Type _eventsServiceType;

        public GetEventsTests()
        {
            _eventsServiceType = Type.GetType("EventManager.Services.Events.EventsService, EventManager.Services");
        }
    }
}
