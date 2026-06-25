using EventManager.Domain.Entities.Users.Enums;
using EventManager.DTOs.Users;
using EventManager.Tests.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

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

        /// <summary>
        /// Resets database and add registers (login = user, password = user) and (login = admin, password = admin)
        /// </summary>
        /// <returns></returns>
        protected async Task SeedDefautDataAsync()
        {
            await ResetDatabaseAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var response1 = await httpClient.PostAsJsonAsync(@"api\auth\register", new RegisterDto("admin", "admin", Roles.Admin), cts.Token);

            var code1 = response1.StatusCode;

            var response2 = await httpClient.PostAsJsonAsync(@"api\auth\register", new RegisterDto("user", "user", Roles.User), cts.Token);

            var code2 = response2.StatusCode;
        }
    }
}
