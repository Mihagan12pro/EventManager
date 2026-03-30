namespace EventManager.Tests.Events.Add
{
    public partial class AddEventsTests
    {
        private readonly Type _eventsServiceType;

        public AddEventsTests()
        {
            _eventsServiceType = Type.GetType("EventManager.Services.Events.EventsService, EventManager.Services");
        }
    }
}
