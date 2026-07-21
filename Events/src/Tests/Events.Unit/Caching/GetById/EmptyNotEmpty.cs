using Events.Application.Handlers.GetByIdEvent;
using Events.Application.Repositories.Cache;
using Events.Application.Repositories.Events;
using Events.Application.Singleton.Cache.Options;
using Events.Domain;
using Events.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using Moq;

namespace Events.Unit.Caching.GetById
{
    public partial class GetByIdCacheTests
    {
        [Fact]
        public async Task Test_NotEmptyCache()
        {
                        CacheKeysOptions cacheKeysOptions = new CacheKeysOptions()
            {
                GetEventKey = new CacheKeyOptions()
                {
                    TTL = 300,

                    Key = "events:event:{0}"
                }
            };

            Event @event = new Event()
            {
                Id = Guid.NewGuid(),

                EventDateTime = new EventDateTime(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2)),

                EventNaming = new EventNaming("Event"),

                Seats = new Seats(10)
            };

            var mockEventsRepository = new Mock<IReadEventsRepository>();
            var mockCacheRepository = new Mock<ICacheRepository>();

            mockCacheRepository.Setup(mock => mock.GetEventAsync(@event.Id, default)).ReturnsAsync(@event);
            mockEventsRepository.Setup(mock => mock.GetEventAsync(@event.Id, default)).ReturnsAsync(@event);

            GetByIdEventCommand command = new GetByIdEventCommand(@event.Id);

            GetByIdEventHandler handler = new GetByIdEventHandler(
                mockCacheRepository.Object,

                mockEventsRepository.Object,

                Options.Create<CacheKeysOptions>(cacheKeysOptions)
            );

            await handler.HandleAsync(command, default);

            mockEventsRepository.Verify(mock => mock.GetEventAsync(command.Id, default), Times.Never);
        }

        [Fact]
        public async Task Test_EmptyCach()
        {
            Event @event = new Event()
            {
                Id = Guid.NewGuid(),

                EventDateTime = new EventDateTime(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2)),

                EventNaming = new EventNaming("Event"),

                Seats = new Seats(10)
            };

            var mockEventsRepository = new Mock<IReadEventsRepository>();
            var mockCacheRepository = new Mock<ICacheRepository>();

            mockEventsRepository.Setup(mock => mock.GetEventAsync(@event.Id, default)).ReturnsAsync(@event);

            GetByIdEventCommand command = new GetByIdEventCommand(@event.Id);

            CacheKeysOptions cacheKeysOptions = new CacheKeysOptions()
            {
                GetEventKey = new CacheKeyOptions()
                {
                    TTL = 300,

                    Key = "events:event:{0}"
                }
            };

            GetByIdEventHandler handler = new GetByIdEventHandler(
                mockCacheRepository.Object,

                mockEventsRepository.Object,

                Options.Create<CacheKeysOptions>(cacheKeysOptions)
            );

            await handler.HandleAsync(command, default);

            mockEventsRepository.Verify(mock => mock.GetEventAsync(command.Id, default), Times.Once);
        }
    }
}
