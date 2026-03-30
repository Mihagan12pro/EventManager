namespace EventManager.Tests.Events.Put
{
    public partial class PutEventsTests
    {
        private readonly Type _eventsServiceType;

        public PutEventsTests()
        {
            _eventsServiceType = Type.GetType("EventManager.Services.Events.EventsService, EventManager.Services");
        }
    }
}
