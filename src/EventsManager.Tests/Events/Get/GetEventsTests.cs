using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using EventManager.Services.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Events.Get
{
    public partial class GetEventsTests
    {
        [Fact]
        [Trait("SubCategory", "Get")]
        public async Task Test_Get_By_Id()
        {
            var cto = new CancellationTokenSource();

            var provider = TestingServicesProvider.GetServicesProvider();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            DateTime datetime = DateTime.Now.AddDays(10);

            var newEvent = new NewEventDto(
                "Юбилей деда",

                 datetime,

                 datetime.AddHours(10), 
                 
                 10);

            Guid id = await eventsService.AddNewAsync(newEvent, cto.Token);
            Guid hiddenId = Guid.Empty;

            var resultSuccessful = await eventsService.GetEventByIdAsync(id, cto.Token);

            Assert.NotNull(resultSuccessful);

            var resultFailed = await Assert.ThrowsAsync<NotFoundException>(() => eventsService.GetEventByIdAsync(hiddenId, cto.Token));
        }

        [Theory]
        [MemberData(nameof(GetAll))]
        [Trait("SubCategory", "Get")]
        public async Task Test_Get_All(
            string? title,
            PaginationDto paginationDto,
            DateTime? start,
            DateTime? end,
            int expectedTotalCount,
            int expectedCountOnPage)
        {
            var cto = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);
            var eventsService = provider.GetRequiredService<IEventsService>();

            await eventsService.AddNewAsync(
                 new NewEventDto(
                     "Юбилей",
                     dateTime.AddDays(1),
                     dateTime.AddDays(2), 
                     10),

                 cto.Token
                 );

            await eventsService.AddNewAsync(
                new NewEventDto(
                    "Юбилей",
                    dateTime.AddDays(1),
                    dateTime.AddDays(2),
                    10),

                 cto.Token
                );

            await eventsService.AddNewAsync(
                new NewEventDto(
                    "Юбилей",
                    dateTime.AddDays(2),
                    dateTime.AddDays(3),
                    10),

                 cto.Token
                );

            await eventsService.AddNewAsync(
                new NewEventDto(
                    "Корпоратив",
                    dateTime.AddDays(2),
                    dateTime.AddDays(3), 
                    10),

                 cto.Token
                );

            var result = await eventsService.GetEventsAsync(
                title,
              
                paginationDto,

                new DateRange(start, end),

                cto.Token
            );

            Assert.Equal(expectedCountOnPage, result.Events.Count());
            Assert.Equal(expectedTotalCount, result.TotalCount);
        }

        [Theory]
        [MemberData(nameof(GetAllWithException))]
        [Trait("SubCategory", "Get")]
        public async Task Test_Fail_Pagination(int page, int pageSize)
        {
            var cto = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            var eventsService = provider.GetRequiredService<IEventsService>();

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => eventsService.GetEventsAsync(
                    string.Empty,
                    new PaginationDto(page, pageSize),
                    new DateRange(null, null),
                    cto.Token
                )
            );
        }
    }
}

