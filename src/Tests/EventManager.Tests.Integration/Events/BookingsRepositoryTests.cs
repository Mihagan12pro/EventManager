using EventManager.DataAccess.PostgreSQL.Events;
using EventManager.DTOs.Events;
using EventManager.Services.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Integration.Events
{
    public class BookingsRepositoryTests : RepositoryTestBase
    {
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
            var provider = await GetServiceProviderAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();

            NewEventDto newEventDto = new NewEventDto("Birthday", DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(1).AddHours(20), 10, "Daddy's birthday");

            var id = await eventsRepository.AddNewAsync(newEventDto, cts.Token);

            await eventsRepository.DeleteAsync(id, cts.Token);

            var @event = await eventsRepository.GetByIdAsync(id, cts.Token);

            Assert.Null(@event);
        }
    }
}
