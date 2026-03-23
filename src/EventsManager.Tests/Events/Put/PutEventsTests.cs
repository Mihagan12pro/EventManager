using EventManager.DomainModels.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Exceptions;
using EventManager.Services.Events;

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

            Guid id = await eventsService.AddNew(
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

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => eventsService.UpdateByPut(id, eventDto));
        }

        //[Fact]
        //public async Task Test_Putting_With_Error_404()
        //{
        //    var eventsService = new EventsService();
        //    Guid id = Guid.NewGuid();

        //    NewEventDto eventDto = new NewEventDto(
        //        string.Empty,
        //        DateTime.Now.AddDays(1),
        //        DateTime.Now.AddDays(2)
        //    );

        //    var result = await eventsService.UpdateByPut(id, eventDto);

        //    Assert.False(result.IsSuccess);
        //    Assert.Equal(404, result.Error.StatusCode);
        //}

        //[Theory]
        //[MemberData(nameof(PutData))]
        //public async Task Test_Putting(int index, NewEventDto eventDto)
        //{
        //    DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);
        //    var eventsService = new EventsService();

        //    await eventsService.AddNew(
        //         new NewEventDto(
        //             "Юбилей",
        //             dateTime.AddDays(1),
        //             dateTime.AddDays(2))
        //         );

        //    await eventsService.AddNew(
        //        new NewEventDto(
        //            "Юбилей",
        //            dateTime.AddDays(1),
        //            dateTime.AddDays(2))
        //        );

        //    await eventsService.AddNew(
        //        new NewEventDto(
        //            "Юбилей",
        //            dateTime.AddDays(2),
        //            dateTime.AddDays(3))
        //        );

        //    await eventsService.AddNew(
        //        new NewEventDto(
        //            "Корпоратив",
        //            dateTime.AddDays(2),
        //            dateTime.AddDays(3))
        //        );

        //    Event oldModel = (await eventsService.GetEvents(
        //        null,
        //        new PaginationDto(),
        //        new DateRange(
        //             null,
        //             false,
        //             null,
        //             false)
        //        )).Events.First();

        //    DateTime start = oldModel.StartAt;
        //    DateTime end = oldModel.EndAt;

        //    string title = oldModel.Title;
        //    string description = oldModel.Description;

        //    var result = (await eventsService.UpdateByPut(oldModel.Id, eventDto));
        //    Event putModel = (await eventsService.GetEvents(
        //        null,
        //        new PaginationDto(),
        //        new DateRange(
        //             null,
        //             false,
        //             null,
        //             false)
        //        )).Events.First();

        //    Assert.True(result.IsSuccess);

        //    Assert.False(title == putModel.Title);
        //    Assert.False(description == putModel.Description);
        //    Assert.False(start == putModel.StartAt);
        //    Assert.False(end == putModel.EndAt);
        //}
    }
}
