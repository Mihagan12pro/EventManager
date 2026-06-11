using EventManager.Tests.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EventsManager.Tests.End2End
{
    public abstract class E2ETests : RealPostgreTests, IClassFixture<EventManagerAppFactory<Program>>
    {
        protected readonly WebApplicationFactory<Program> factory;
        protected readonly HttpClient httpClient;

        public E2ETests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory;
            httpClient = factory.CreateClient();
        }
    }
}
