using EventManager.DataAccess.PostgreSQL.Events;
using EventManager.DTOs.Events;

namespace EventManager.Tests.Integration.Events
{
    public class BookingsRepositoryTests : RepositoryTestBase
    {
        [Fact]
        public async Task Test_AddingNew()
        {
            await ResetDatabaseAsync();
            await using var context = CreateContext();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = new PostgreEventsRepository(context);

            NewEventDto newEventDto = new NewEventDto("Birthday", DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(1).AddHours(20), 10, "Daddy's birthday");

            var id = await eventsRepository.AddNewAsync(newEventDto, cts.Token);

            var @event = await eventsRepository.GetByIdAsync(id, cts.Token);

            Assert.NotNull(@event);
        }

        [Fact]
        public async Task Test_GetDeletedEvent()
        {
            await ResetDatabaseAsync();
            await using var context = CreateContext();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = new PostgreEventsRepository(context);

            NewEventDto newEventDto = new NewEventDto("Birthday", DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(1).AddHours(20), 10, "Daddy's birthday");

            var id = await eventsRepository.AddNewAsync(newEventDto, cts.Token);

            await eventsRepository.DeleteAsync(id, cts.Token);

            var @event = await eventsRepository.GetByIdAsync(id, cts.Token);

            Assert.Null(@event);
        }
    }
}
