using EventManager.DomainModels.Events;
using EventManager.DTOs.Events;
using StatusCodes = EventManager.Shared.StatusCodes;

namespace EventsManager.Tests.Events.Put
{
    public partial class PutEventsTests
    {
        [Theory]
        [MemberData(nameof(PutDataForBadRequest))]
        public async Task Test_Putting_With_Error_400(
            DateTime start,
            DateTime end)
        {
            Guid id = (await _eventsSeeder.GetSeededData())
                .First().Id;

            NewEventDto eventDto = new NewEventDto(
                string.Empty,
                start,
                end
            );

            var result = await _eventsService.UpdateByPut(id, eventDto);

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.BadRequest, result.Error.StatusCode);
        }

        [Fact]
        public async Task Test_Putting_With_Error_404()
        {
            Guid id = Guid.NewGuid();

            NewEventDto eventDto = new NewEventDto(
                string.Empty, 
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2)
            );

            var result = await _eventsService.UpdateByPut(id, eventDto);

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.NotFound, result.Error.StatusCode);
        }

        [Theory]
        [MemberData(nameof(PutData))]
        public async Task Test_Putting(int index, NewEventDto eventDto)
        {
            Event oldModel = (await _eventsSeeder.GetSeededData()).First();

            DateTime start = oldModel.StartAt;
            DateTime end = oldModel.EndAt;

            string title = oldModel.Title;
            string description = oldModel.Description;

            var result = (await _eventsService.UpdateByPut(oldModel.Id, eventDto));
            Event putModel = (await _eventsSeeder.GetSeededData()).First();

            Assert.True(result.IsSuccess);

            Assert.False(title == putModel.Title);
            Assert.False(description == putModel.Description);
            Assert.False(start == putModel.StartAt);
            Assert.False(end == putModel.EndAt);
        }
    }
}
