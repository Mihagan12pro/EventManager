using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;

namespace EventManager.Tests.Events.Put
{
    public partial class PutEventsTests
    {
        //[Theory]
        //[MemberData(nameof(PutDataForBadRequest))]
        //public async Task Test_Putting_Bad_Request(
        //    DateTime start,
        //    DateTime end)
        //{
        //    CancellationTokenSource cts = new CancellationTokenSource();

        //    DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);
        //    var eventsService = (IEventsService)Activator.CreateInstance(_eventsServiceType);

        //    Guid id = await eventsService.AddNewAsync(
        //         new NewEventDto(
        //             "Юбилей",
        //             dateTime.AddDays(1),
        //             dateTime.AddDays(2), 
        //             10),
        //         cts.Token
        //         );

        //    NewEventDto eventDto = new NewEventDto(
        //        string.Empty,
        //        start,
        //        end, 10
        //    );

        //    var exception = await Assert.ThrowsAsync<BadRequestException>(() => eventsService.UpdateByPutAsync(id, eventDto, cts.Token));
        //}

        [Fact]
        public async Task Test_Putting_With_Error_404()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsService = (IEventsService)Activator.CreateInstance(_eventsServiceType);
            Guid id = Guid.NewGuid();

            PutEventDto eventDto = new PutEventDto(
                "Birthday",
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2)
            );

            var result = await Assert.ThrowsAsync<NotFoundException>(() => eventsService.UpdateByPutAsync(id, eventDto, cts.Token));
        }

        //[Theory]
        //[MemberData(nameof(PutData))]
        //public async Task Test_Putting(int index, NewEventDto eventDto)
        //{
        //    CancellationTokenSource cts = new CancellationTokenSource();

        //    DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);
        //    var eventsService = (IEventsService)Activator.CreateInstance(_eventsServiceType);

        //    await eventsService.AddNewAsync(
        //         new NewEventDto(
        //             "Юбилей",
        //             dateTime.AddDays(1),
        //             dateTime.AddDays(2),
        //             10),
        //         cts.Token
        //         );

        //    EventModel oldModel = (await eventsService.GetEventsAsync(
        //        null,
        //        new PaginationDto(1, 10),
        //        new DateRange(
        //             null,
        //             null)
        //        )).Events.First();

        //    DateTime start = oldModel.StartAt;
        //    DateTime end = oldModel.EndAt;

        //    string title = oldModel.Title;
        //    string description = oldModel.Description;

        //    var result = (await eventsService.UpdateByPutAsync(oldModel.Id, eventDto));
        //    EventModel putModel = (await eventsService.GetEventsAsync(
        //        null,
        //        new PaginationDto(),
        //        new DateRange(
        //             null,
        //             false,
        //             null,
        //             false)
        //        )).Events.First();

        //    Assert.False(title == putModel.Title);
        //    Assert.False(description == putModel.Description);
        //    Assert.False(start == putModel.StartAt);
        //    Assert.False(end == putModel.EndAt);
        //}
    }
}
