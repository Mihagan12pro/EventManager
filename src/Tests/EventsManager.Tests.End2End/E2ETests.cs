using EventManager.Domain.Entities.Users.Enums;
using EventManager.DTOs.Users;
using System.Net.Http.Json;
using System.Text.Json;

namespace EventsManager.Tests.End2End
{
    public abstract class E2ETests : IClassFixture<EventManagerAppFactory<Program>>
    {
        protected readonly EventManagerAppFactory<Program> factory;
        protected readonly HttpClient httpClient;

        protected JsonSerializerOptions serializerOptions;

        public E2ETests(EventManagerAppFactory<Program> factory)
        {
            this.factory = factory;
            httpClient = factory.CreateClient();

            serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        /// <summary>
        /// Resets database and add registers (login = user, password = user) and (login = admin, password = admin)
        /// </summary>
        /// <returns></returns>
        protected async Task SeedDefautDataAsync()
        {
            await factory.ResetDatabaseAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var response1 = await httpClient.PostAsJsonAsync(@"api\auth\register", new RegisterDto("admin", "admin", Roles.Admin), cts.Token);

            var code1 = response1.StatusCode;

            var response2 = await httpClient.PostAsJsonAsync(@"api\auth\register", new RegisterDto("user", "user", Roles.User), cts.Token);

            var code2 = response2.StatusCode;
        }
    }
}
