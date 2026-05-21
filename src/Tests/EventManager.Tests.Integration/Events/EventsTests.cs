using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

namespace EventManager.Tests.Integration.Events
{
    public class EventsTests : IntegrationTests
    {
        public EventsTests(EventManagerAppFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Test_AddingNew()
        {
            await ResetDatabaseAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);

            var response = await httpClient.PostAsJsonAsync(@"events\", newEvent, cts.Token);

            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.Created);
            string responseBody = await response.Content.ReadAsStringAsync();

            Guid.TryParse(responseBody.Trim('"'), out Guid id);

            Assert.NotEqual(Guid.Empty, id);
        }

        [Fact]
        public async Task Test_GetDeletedEvent()
        {
            await ResetDatabaseAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);

            var postResponse = await httpClient.PostAsJsonAsync(@"events\", newEvent, cts.Token);

            string responseBody = (await postResponse.Content.ReadAsStringAsync()).Trim('"');

            Guid.TryParse(responseBody.Trim('"'), out Guid id);

            var deleteResponse = await httpClient.DeleteAsync(@$"events\{id}");

            Assert.Equal(deleteResponse.StatusCode, HttpStatusCode.OK);

            var getResult = await httpClient.GetAsync($@"events\{id}");

            Assert.Equal(getResult.StatusCode, HttpStatusCode.NotFound);
        }
    }
}
