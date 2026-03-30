using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Services.Exceptions;

namespace EventManager.Tests.Events.Add
{
    public partial class AddEventsTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Successful_Adding(NewEventDto newEventDto)
        {
            IEventsService eventsService = (IEventsService)Activator.CreateInstance(_eventsServiceType);

            var result = await eventsService.AddNewAsync(newEventDto);
            var deletingResult = await eventsService.DeleteAsync(result);

            Assert.Equal(typeof(Guid), result.GetType());
            Assert.Equal(typeof(string), deletingResult.GetType());
        }

        [Theory]
        [MemberData(nameof(AddBadRequest))]
        public async Task Test_Bad_Request(NewEventDto dto, string expected)
        {
            EventsService eventsService = new EventsService();

            var result = await Assert.ThrowsAsync<BadRequestException>(() => eventsService.AddNewAsync(dto));
            Assert.Equal(expected, result.Error.Message);
        }
    }
}
