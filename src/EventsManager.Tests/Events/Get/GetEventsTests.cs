using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Services.Events;
using EventManager.Services.Exceptions;

namespace EventsManager.Tests.Events.Get
{
    [Collection("Get events collection")]
    public partial class GetEventsTests
    {
        [Fact]
        public async Task Test_Get_By_Id()
        {
            var eventsService = new EventsService();
            DateTime datetime = DateTime.Now.AddDays(1);

            var newEvent = new NewEventDto(
                "Юбилей деда",

                 datetime,

                 datetime.AddHours(10));

            Guid id = await eventsService.AddNewAsync(newEvent);
            Guid hiddenId = Guid.Empty;

            var resultSuccessful = (await eventsService.GetEventByIdAsync(id));
          
            Assert.Equal(typeof(GetEventDto), resultSuccessful.GetType());

            var resultFailed = await Assert.ThrowsAsync<NotFoundException>(() => eventsService.GetEventByIdAsync(hiddenId));

            await eventsService.DeleteAsync(id);
        }

        [Theory]
        [MemberData(nameof(GetAll))]
        public async Task Test_Get_All(
            string? title,
            PaginationDto paginationDto,
            DateTime? start,
            DateTime? end,
            int expectedTotalCount,
            int expectedCountOnPage)
        {
            DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);
            var eventsService = new EventsService();

            await eventsService.AddNewAsync(
                 new NewEventDto(
                     "Юбилей",
                     dateTime.AddDays(1),
                     dateTime.AddDays(2))
                 );

            await eventsService.AddNewAsync(
                new NewEventDto(
                    "Юбилей",
                    dateTime.AddDays(1),
                    dateTime.AddDays(2))
                );

            await eventsService.AddNewAsync(
                new NewEventDto(
                    "Юбилей",
                    dateTime.AddDays(2),
                    dateTime.AddDays(3))
                );

            await eventsService.AddNewAsync(
                new NewEventDto(
                    "Корпоратив",
                    dateTime.AddDays(2),
                    dateTime.AddDays(3))
                );

            var result = await eventsService.GetEventsAsync(
              title,
              paginationDto,
              new DateRange(
                  start,
                  false,
                  end,
                  false)
              );

            Assert.Equal(expectedCountOnPage, result.Events.Count);
            Assert.Equal(expectedTotalCount, result.TotalCount);
        }

        [Theory]
        [MemberData(nameof(GetAllWithException))]
        public async Task Test_Fail_Pagination(int page, int pageSize)
        {
            var eventsService = new EventsService();

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => eventsService.GetEventsAsync(
                    string.Empty,
                    new PaginationDto(page, pageSize),
                    new DateRange(null, false, null, false)
                )
            );
        }
    }
}

