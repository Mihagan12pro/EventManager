using EventManager.DTOs.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using EventManager.Domain.ValueObjects;
using EventManager.Shared.Filters;
using EventManager.Application.DataAccess.Repositories;
using EventManager.Domain.Entities.Events;

namespace EventManager.Tests.Integration.CRUD.Events
{
    public partial class EventsCrudTests : IntegrationTests
    {
        [Fact]
        public async Task Test_AddNewAsync()
        {
            await ResetDatabaseAsync();
            var provider = await GetServiceProviderAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();

            NewEventDto eventDto = new NewEventDto("Fiest", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddHours(20), 20);
            Guid id = await eventsRepository.AddNewAsync(eventDto, cts.Token);

            Assert.NotNull(id);
        }

        [Fact]
        public async Task Test_GetByIdAsync()
        {
            await ResetDatabaseAsync();
            var provider = await GetServiceProviderAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();

            NewEventDto newEventDto = new NewEventDto("Fiest", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddHours(20), 20);
            Guid id = await eventsRepository.AddNewAsync(newEventDto, cts.Token);

            var @event = await eventsRepository.GetByIdAsync(id, cts.Token);
            Assert.NotNull(@event);
        }

        [Fact]
        public async Task Test_DeleteAsync()
        {
            await ResetDatabaseAsync();
            var provider = await GetServiceProviderAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();

            NewEventDto newEventDto = new NewEventDto("Fiest", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddHours(20), 20);
            Guid id = await eventsRepository.AddNewAsync(newEventDto, cts.Token);

            await eventsRepository.DeleteAsync(id, cts.Token);

            var @event = await eventsRepository.GetByIdAsync(id, cts.Token);

            Assert.Null(@event);
        }

        [Fact]
        public async Task Test_CompleteUpdateAsync()
        {
            await ResetDatabaseAsync();
            var provider = await GetServiceProviderAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();

            NewEventDto newEventDto = new NewEventDto("Fiest", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddHours(20), 20, "Something");
            Guid id = await eventsRepository.AddNewAsync(newEventDto, cts.Token);

            PutEventDto putEventDto = new PutEventDto("Birthday", DateTime.UtcNow.AddYears(2), DateTime.UtcNow.AddYears(2).AddHours(20));
            await eventsRepository.CompleteUpdateAsync(id, putEventDto, cts.Token);

            var updatedEvent = await eventsRepository.GetByIdAsync(id, cts.Token);

            Assert.NotEqual(newEventDto.Title, updatedEvent.Title);
            Assert.NotEqual(newEventDto.StartAt, updatedEvent.StartAt);
            Assert.NotEqual(newEventDto.EndAt, updatedEvent.EndAt);
            Assert.NotEqual(newEventDto.Description, updatedEvent.Description);
        }

        [Theory]
        [MemberData(nameof(Filters))]
        public async Task Test_GetPaginatedEventsAsync(
            Expression<Func<EventEntity, bool>> filters, 
            Pagination pagination, 
            int expected)
        {
            await ResetDatabaseAsync();
            var provider = await GetServiceProviderAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();

            foreach (var seed in Seed())
            {
                await eventsRepository.AddNewAsync(seed, cts.Token);
            }

            var dto = await eventsRepository.GetPaginatedEventsAsync(new Filters<EventEntity> { filters }, pagination, cts.Token);

            Assert.Equal(expected, dto.TotalCount);
        }
    }
}
