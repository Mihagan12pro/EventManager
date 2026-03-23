using EventManager.DTOs.Events;
using EventManager.Exceptions;
using EventManager.Services.Events;

namespace EventsManager.Tests.Events.Add
{
    public partial class AddEventsTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Successful_Adding(NewEventDto newEventDto)
        {
            EventsService eventsService = new EventsService();

            var result = await eventsService.AddNew(newEventDto);
            var deletingResult = await eventsService.Delete(result);

            Assert.Equal(typeof(Guid), result.GetType());
            Assert.Equal(typeof(string), deletingResult.GetType());
        }

        [Theory]
        [MemberData(nameof(AddBadRequest))]
        public async Task Test_Bad_Request(NewEventDto dto, string expected)
        {
            EventsService eventsService = new EventsService();

            var result = await Assert.ThrowsAsync<BadRequestException>(() => eventsService.AddNew(dto));
            Assert.Equal(expected, result.Error.Message);
        }
    }
}
