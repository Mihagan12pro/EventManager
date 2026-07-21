using Events.Application.Dtos;
using Events.Application.Handlers.CompleteUpdate;
using Events.Application.Handlers.GetByIdEvent;
using Events.Application.Repositories.Events;
using Events.Application.Singleton.Cache.Options;
using Events.Domain;
using Events.Domain.ValueObjects;
using Events.Unit.Caching.Utils;
using Microsoft.Extensions.Options;
using Moq;

namespace Events.Unit.Caching.GetById
{
    public partial class GetByIdCacheTests
    {
        [Fact]
        public async Task Test_Update()
        {
            Event @event = new Event()
            {
                Id = Guid.NewGuid(),

                EventDateTime = new EventDateTime(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2)),

                EventNaming = new EventNaming("Event"),

                Seats = new Seats(10)
            };

            var mockReadEventsRepository = new Mock<IReadEventsRepository>();
            var mockWriteEventsRepository = new Mock<IWriteEventsRepository>();
            var mockCacheRepository = new FakeCacheRepository();

            mockReadEventsRepository.Setup(mock => mock.GetEventAsync(@event.Id, default)).ReturnsAsync(@event);

            GetByIdEventCommand getCommand = new GetByIdEventCommand(@event.Id);
            CompleteEventUpdateCommand updateCommand = new CompleteEventUpdateCommand(
                @event.Id,
                new UpdateEventDto(
                    "qwe", 
                    "qwe",
                    DateTime.UtcNow.AddDays(1),
                    DateTime.UtcNow.AddDays(2)
                )
            );

            CacheKeysOptions cacheKeysOptions = new CacheKeysOptions()
            {
                GetEventKey = new CacheKeyOptions()
                {
                    TTL = 300,

                    Key = "events:event:{0}"
                }
            };

            GetByIdEventHandler getHandler = new GetByIdEventHandler(
                mockCacheRepository,

                mockReadEventsRepository.Object,

                Options.Create<CacheKeysOptions>(cacheKeysOptions)
            );

            CompleteEventUpdateHandler updateHandler = new CompleteEventUpdateHandler(mockCacheRepository, mockWriteEventsRepository.Object);

            await getHandler.HandleAsync(getCommand, default);
            bool containsBeforeUpdate = await mockCacheRepository.CheckKeyAsync(cacheKeysOptions.GetEventKey.FormatKey(@event.Id), default);

            await updateHandler.HandleAsync(updateCommand, default);
            bool containsAfterUpdate = await mockCacheRepository.CheckKeyAsync(cacheKeysOptions.GetEventKey.FormatKey(@event.Id), default);

            Assert.True(containsBeforeUpdate);
            Assert.False(containsAfterUpdate);
        }
    }
}
