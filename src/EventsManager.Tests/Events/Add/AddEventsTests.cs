using EventManager.DTOs.Events;

namespace EventsManager.Tests.Events.Add
{
    public partial class AddEventsTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Adding_New_Events_With_Correct_DateTime(NewEventDto newEventDto)
        {
            if (newEventDto.StartAt.Value.CompareTo(newEventDto.EndAt.Value) < 0)
            {
                var result = await _eventsService.AddNew(newEventDto);

                Assert.True(result.IsSuccess);

                await _eventsService.Delete(result.Value);
            }
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Adding_New_Events_With_InCorrect_DateTime(NewEventDto newEventDto)
        {
            var value = newEventDto.StartAt.Value.CompareTo(newEventDto.EndAt.Value);

            if (newEventDto.StartAt.Value.CompareTo(newEventDto.EndAt.Value) >= 0)
            {
                var result = await _eventsService.AddNew(newEventDto);
                Assert.False(result.IsSuccess);
            }
        }

        [Theory]
        [MemberData(nameof(AddEventsWithException))]
        public async Task Test_Add_Events_With_Exception(NewEventDto dto, string expected)
        {
            var result = await Assert.ThrowsAsync<EventManager.Exceptions.BadRequestException>(() => _eventsService.AddNew(dto));
            Assert.Equal(expected, result.Error.Message);
        }
    }
}
