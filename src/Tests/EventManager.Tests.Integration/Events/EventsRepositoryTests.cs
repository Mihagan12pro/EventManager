using EventManager.DTOs.Events;
using EventManager.Services.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;

namespace EventManager.Tests.Integration.Events
{
    public class EventsRepositoryTests : RepositoryTestBase
    {
        public EventsRepositoryTests(EventManagerAppFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Test_AddingNew()
        {
            await ResetDatabaseAsync();
            var provider = await GetServiceProviderAsync();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();
            CancellationTokenSource cts = new CancellationTokenSource();

            NewEventDto newEventDto = new NewEventDto("Birthday", DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(1).AddHours(20), 10, "Daddy's birthday");

            var id = await eventsRepository.AddNewAsync(newEventDto, cts.Token);

            var @event = await eventsRepository.GetByIdAsync(id, cts.Token);

            Assert.NotNull(@event);
        }

        [Fact]
        public async Task Test_GetDeletedEvent()
        {
            await ResetDatabaseAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), 10);

            var response = httpClient.PostAsJsonAsync(@"events\", newEvent, cts.Token);


        }
    }
}
