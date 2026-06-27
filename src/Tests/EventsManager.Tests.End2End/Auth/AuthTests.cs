using EventManager.Domain.Entities.Users.Enums;
using EventManager.DTOs.Events;
using EventManager.DTOs.Users;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EventsManager.Tests.End2End.Auth
{
    public class AuthTests : E2ETests
    {
        public AuthTests(EventManagerAppFactory<Program> factory) : base(factory)
        {
        }


        [Fact]
        public async Task Test_RegisterUsers()
        {
            await SeedDefautDataAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var response1 = await httpClient.PostAsJsonAsync(@"api\auth\register", new RegisterDto("Mihagan12Pro", "password", Roles.Admin));
            Assert.Equal(HttpStatusCode.NoContent, response1.StatusCode);

            var response2 = await httpClient.PostAsJsonAsync(@"api\auth\register", new RegisterDto("Mihagan12Pro", "password", Roles.User));
            Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);

            var response3 = await httpClient.PostAsJsonAsync(@"api\auth\register", new RegisterDto("", "password", Roles.User));
            Assert.Equal(HttpStatusCode.BadRequest, response3.StatusCode);
        }

        [Fact]
        public async Task Test_LoginIncorrectPassword()
        {
            await SeedDefautDataAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var response1 = await httpClient.PostAsJsonAsync(@"api\auth\register", new RegisterDto("Mihagan12Pro", "password", Roles.Admin));
            Assert.Equal(HttpStatusCode.NoContent, response1.StatusCode);

            var response2 = await httpClient.PostAsJsonAsync(@"api\auth\login", new RegisterDto("Mihagan12Pro", "1234567890"));
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
        }

        [Fact]
        public async Task Test_Forbidden()
        {
            await SeedDefautDataAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var loginReponse = await httpClient.PostAsJsonAsync(@"api\auth\login", new LoginDto("user", "user"), cts.Token);

            string token = await loginReponse.Content.ReadAsStringAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);
            var userCreateEventResponse = await httpClient.PostAsJsonAsync(@"api\events\", newEvent, cts.Token);

            Assert.Equal(HttpStatusCode.Forbidden, userCreateEventResponse.StatusCode);
        }

        [Fact]
        public async Task Test_Unauthorized()
        {
            await SeedDefautDataAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);
            
            var createEventResponse = await httpClient.PostAsJsonAsync(@"api\events\", newEvent, cts.Token);
            var getBookingResponse = await httpClient.GetAsync(@$"api\bookings\{Guid.NewGuid()}", cts.Token);
            var cancelBookingResponse = await httpClient.DeleteAsync(@$"api\bookings\{Guid.NewGuid()}", cts.Token);

            Assert.Equal(HttpStatusCode.Unauthorized, createEventResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, getBookingResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, cancelBookingResponse.StatusCode);
        }
    }
}
