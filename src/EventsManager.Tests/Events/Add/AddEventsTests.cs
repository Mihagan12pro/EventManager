using EventManager.DTOs.Events;
using EventManager.Services.Events;

namespace EventsManager.Tests.Events.Add
{
    public partial class AddEventsTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Adding_New_Events_With_Correct_DateTime(NewEventDto newEventDto)
        {
            EventsService eventsService = new EventsService();

            if (newEventDto.StartAt.Value.CompareTo(newEventDto.EndAt.Value) < 0)
            {
                var result = await eventsService.AddNew(newEventDto);

                Assert.True(result.IsSuccess);

                await eventsService.Delete(result.Value);
            }
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Adding_New_Events_With_InCorrect_DateTime(NewEventDto newEventDto)
        {
            EventsService eventsService = new EventsService();

            var value = newEventDto.StartAt.Value.CompareTo(newEventDto.EndAt.Value);

            if (newEventDto.StartAt.Value.CompareTo(newEventDto.EndAt.Value) >= 0)
            {
                var result = await eventsService.AddNew(newEventDto);
                Assert.False(result.IsSuccess);
            }
        }

        [Theory]
        [MemberData(nameof(AddEventsWithException))]
        public async Task Test_Add_Events_With_Exception(NewEventDto dto, string expected)
        {
            EventsService eventsService = new EventsService();

            var result = await Assert.ThrowsAsync<EventManager.Exceptions.BadRequestException>(() => eventsService.AddNew(dto));
            Assert.Equal(expected, result.Error.Message);
        }
    }
}
