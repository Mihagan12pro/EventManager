using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;

namespace EventManager.Tests.Events.Add
{
    public partial class AddEventsTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "Add")]
        public async Task Test_Successful_Adding(NewEventDto newEventDto)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            IEventsService eventsService = (IEventsService)Activator.CreateInstance(_eventsServiceType);

            var result = await eventsService.AddNewAsync(newEventDto, cts.Token);
            var deletingResult = await eventsService.DeleteAsync(result, cts.Token);

            Assert.Equal(typeof(Guid), result.GetType());
            Assert.Equal(typeof(string), deletingResult.GetType());
        }

        [Theory]
        [MemberData(nameof(AddBadRequest))]
        [Trait("SubCategory", "Add")]
        public async Task Test_Bad_Request(NewEventDto dto, string expected)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            IEventsService eventsService = (IEventsService)Activator.CreateInstance(_eventsServiceType);

            var result = await Assert.ThrowsAsync<BadRequestException>(() => eventsService.AddNewAsync(dto, cts.Token));
            Assert.Equal(expected, result.Error.Message);
        }
    }
}
