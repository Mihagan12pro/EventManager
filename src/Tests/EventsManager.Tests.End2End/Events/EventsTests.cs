using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.DTOs.Users;
using EventsManager.Tests.End2End.Extensions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

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

            var loginReponse = await httpClient.PostAsJsonAsync(@"api\auth\login", new LoginDto("admin", "admin"), cts.Token);

            string token = await loginReponse.Content.ReadAsStringAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);

            var response = await httpClient.PostAsJsonAsync(@"api\events\", newEvent, cts.Token);

            Assert.Equal(response.StatusCode, HttpStatusCode.Created);
            string responseBody = await response.Content.ReadAsStringAsync();

            Guid.TryParse(responseBody.Trim('"'), out Guid id);

            Assert.NotEqual(Guid.Empty, id);
        }
    }
}
