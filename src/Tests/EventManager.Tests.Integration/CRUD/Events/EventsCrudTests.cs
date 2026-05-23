using Microsoft.AspNetCore.Mvc.Testing;

namespace EventManager.Tests.Integration.CRUD.Events
{
    public class EventsCrudTests : IntegrationTests
    {
        public EventsCrudTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }
    }
}
