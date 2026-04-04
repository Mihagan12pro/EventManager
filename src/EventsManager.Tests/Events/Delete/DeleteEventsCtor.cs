using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Tests.Events.Delete
{
    public partial class DeleteEventsTests : EventsTests
    {
        private readonly Type _eventsServiceType;

        public DeleteEventsTests()
        {
            _eventsServiceType = Type.GetType("EventManager.Services.Events.EventsService, EventManager.Services");
        }
    }
}
