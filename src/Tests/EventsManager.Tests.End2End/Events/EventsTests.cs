using EventManager.DTOs.Events;
using EventManager.DTOs.Users;
using System.Net.Http.Json;

namespace EventsManager.Tests.End2End.Events
{
    public class EventsTests : E2ETests
    {
        public EventsTests(EventManagerAppFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Test_AddingNew()
        {
            await SeedDefautDataAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var token = await httpClient.PostAsJsonAsync(@"api\auth\login", new LoginDto("admin", "admin"), cts.Token);

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);

            var response = await httpClient.PostAsJsonAsync(@"api\events\", newEvent, cts.Token);

            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.Created);
            string responseBody = await response.Content.ReadAsStringAsync();

            Guid.TryParse(responseBody.Trim('"'), out Guid id);

            Assert.NotEqual(Guid.Empty, id);
        }

        //[Fact]
        //public async Task Test_GetDeletedEvent()
        //{
        //    await ResetDatabaseAsync();

        //    CancellationTokenSource cts = new CancellationTokenSource();

        //    NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);

        //    var postResponse = await httpClient.PostAsJsonAsync(@"api\events\", newEvent, cts.Token);

        //    string responseBody = (await postResponse.Content.ReadAsStringAsync()).Trim('"');

        //    Guid.TryParse(responseBody.Trim('"'), out Guid id);

        //    var deleteResponse = await httpClient.DeleteAsync(@$"api\events\{id}");

        //    Assert.Equal(deleteResponse.StatusCode, HttpStatusCode.OK);

        //    var getResult = await httpClient.GetAsync($@"api\events\{id}");

        //    Assert.Equal(getResult.StatusCode, HttpStatusCode.NotFound);
        //}
    }
}
