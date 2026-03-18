using EventManager.DomainModels.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;

namespace EventsManager.Tests.Events.Get
{
    public partial class GetEventsTests
    {
        [Fact]
        public async Task Test_Get_By_Id()
        {
            DateTime datetime = DateTime.Now.AddDays(1);

            var newEvent = new NewEventDto(
                        "Юбилей деда",

                        datetime,

                        datetime.AddHours(10));

            var result1 = await _eventsService.AddNew(newEvent);

            Guid id = result1.Value;
            Guid hiddenId = Guid.Empty;

            var result2 = (await _eventsService.GetEventById(id)).Value;
            var result3 = (await _eventsService.GetEventById(hiddenId)).Error;

            Assert.Equal(typeof(GetEventDto), result2.GetType());
            Assert.Equal(typeof(string), result3.GetType());
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
            if (!IsSeedAdded)
            {
                await _eventsSeeder.AddSeedData();

                IsSeedAdded = true;
            }

            var result = await _eventsService.GetEvents(title, paginationDto, new DateRange(start, false, end, false));

            Assert.Equal(expectedCountOnPage, result.Events.Count);
            Assert.Equal(expectedTotalCount, result.TotalCount);
        }

    }
}
