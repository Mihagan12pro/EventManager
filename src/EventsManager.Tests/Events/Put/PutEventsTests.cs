using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Services.Events;
using EventManager.Services.Exceptions;

namespace EventsManager.Tests.Events.Put
{
    [Collection("Put events collection")]
    public partial class PutEventsTests
    {
        [Theory]
        [MemberData(nameof(PutDataForBadRequest))]
        public async Task Test_Putting_Bad_Request(
            DateTime start,
            DateTime end)
        {
            DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);
            var eventsService = new EventsService();

            Guid id = await eventsService.AddNewAsync(
                 new NewEventDto(
                     "Юбилей",
                     dateTime.AddDays(1),
                     dateTime.AddDays(2))
                 );

            NewEventDto eventDto = new NewEventDto(
                string.Empty,
                start,
                end
            );

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => eventsService.UpdateByPutAsync(id, eventDto));
        }

        [Fact]
        public async Task Test_Putting_With_Error_404()
        {
            var eventsService = new EventsService();
            Guid id = Guid.NewGuid();

            NewEventDto eventDto = new NewEventDto(
                string.Empty,
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2)
            );

            var result = await Assert.ThrowsAsync<NotFoundException>(() => eventsService.UpdateByPutAsync(id, eventDto));
        }

        [Theory]
        [MemberData(nameof(PutData))]
        public async Task Test_Putting(int index, NewEventDto eventDto)
        {
            DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);
            var eventsService = new EventsService();

            await eventsService.AddNewAsync(
                 new NewEventDto(
                     "Юбилей",
                     dateTime.AddDays(1),
                     dateTime.AddDays(2))
                 );

            Event oldModel = (await eventsService.GetEventsAsync(
                null,
                new PaginationDto(),
                new DateRange(
                     null,
                     false,
                     null,
                     false)
                )).Events.First();

            DateTime start = oldModel.StartAt;
            DateTime end = oldModel.EndAt;

            string title = oldModel.Title;
            string description = oldModel.Description;

            var result = (await eventsService.UpdateByPutAsync(oldModel.Id, eventDto));
            Event putModel = (await eventsService.GetEventsAsync(
                null,
                new PaginationDto(),
                new DateRange(
                     null,
                     false,
                     null,
                     false)
                )).Events.First();

            Assert.False(title == putModel.Title);
            Assert.False(description == putModel.Description);
            Assert.False(start == putModel.StartAt);
            Assert.False(end == putModel.EndAt);
        }
    }
}
