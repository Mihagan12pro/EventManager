using EventManager.DomainModels.Events;
using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Shared;

namespace EventsManager.Tests.Events.Delete
{
    public partial class DeleteEventsTests
    {
        [Theory]
        [MemberData(nameof(PutDataForBadRequest))]
        public async Task Test_Putting_With_Error_400(
           DateTime start,
           DateTime end)
        {
            var dateTime = DateTime.Now.AddDays(1);
            var eventsService = new EventsService();

            var dto = new NewEventDto(
                   "Юбилей",
                   dateTime.AddDays(1),
                   dateTime.AddDays(2));

            Guid id = (await eventsService.AddNew(dto)).Value;

            NewEventDto newDto = new NewEventDto(
                string.Empty,
                start,
                end
            );

            var result = await eventsService.UpdateByPut(id, newDto);

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.BadRequest, result.Error.StatusCode);
        }

        [Fact]
        public async Task Test_Putting_With_Error_404()
        {
            var dateTime = DateTime.Now.AddDays(1);
            var eventsService = new EventsService();


            Guid id = Guid.NewGuid();

            NewEventDto dto = new NewEventDto(
                string.Empty,
                dateTime.AddDays(1),
                dateTime.AddDays(2)
            );

            var result = await eventsService.UpdateByPut(id, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(StatusCodes.NotFound, result.Error.StatusCode);
        }

        [Theory]
        [MemberData(nameof(PutData))]
        public async Task Test_Putting(int index, NewEventDto eventDto)
        {
            var dateTime = DateTime.Now.AddDays(1);
            var eventsService = new EventsService();

            var dto = new NewEventDto(
                   "Юбилей",
                   dateTime.AddDays(1),
                   dateTime.AddDays(2));

            Guid id = (await eventsService.AddNew(dto)).Value;

            var result = (await eventsService.UpdateByPut(id, eventDto));

            var getDto = (await eventsService.GetEventById(id)).Value;

            Assert.True(result.IsSuccess);

            Assert.False(dto.Title == getDto.Title);
            Assert.False(dto.Description == getDto.Description);
            Assert.False(dto.StartAt == getDto.StartAt);
            Assert.False(dto.EndAt == getDto.EndAt);
        }
    }
}

