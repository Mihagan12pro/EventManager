using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Unit.Events.Put
{
    public partial class PutEventsTests
    {
        [Theory]
        [MemberData(nameof(PutDataForBadRequest))]
        public async Task Test_Putting_Bad_Request(
            DateTime start,
            DateTime end)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);

            var provider = TestingServicesProvider.GetServicesProvider();
            var eventsService = provider.GetRequiredService<IEventsService>();

            Guid id = await eventsService.AddNewAsync(
                 new NewEventDto(
                     "Юбилей",
                     dateTime.AddDays(1),
                     dateTime.AddDays(2),
                     10),
                 cts.Token
                 );

            PutEventDto putEventDto = new PutEventDto(
                string.Empty,
                start,
                end
            );

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => eventsService.UpdateByPutAsync(id, putEventDto, cts.Token));
        }

        [Fact]
        public async Task Test_Putting_With_Error_404()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var provider = TestingServicesProvider.GetServicesProvider();
            var eventsService = provider.GetRequiredService<IEventsService>();

            Guid id = Guid.NewGuid();

            PutEventDto eventDto = new PutEventDto(
                "Birthday",
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2)
            );

            var result = await Assert.ThrowsAsync<NotFoundException>(() => eventsService.UpdateByPutAsync(id, eventDto, cts.Token));
        }

        [Theory]
        [MemberData(nameof(PutData))]
        public async Task Test_Putting(NewEventDto eventDto)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var provider = TestingServicesProvider.GetServicesProvider();
            var eventsService = provider.GetRequiredService<IEventsService>();

            DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);

            var id = await eventsService.AddNewAsync(eventDto, cts.Token);

            var oldModel = await eventsService.GetEventByIdAsync(id, cts.Token);

            await eventsService.UpdateByPutAsync(id, new PutEventDto(eventDto.Title, eventDto.StartAt, dateTime.AddYears(100)), cts.Token);

            var updatedModel = await eventsService.GetEventByIdAsync(id, cts.Token);

            Assert.NotEqual(oldModel, updatedModel);
        }
    }
}
