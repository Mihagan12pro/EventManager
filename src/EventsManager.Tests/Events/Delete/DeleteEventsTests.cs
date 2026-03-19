using EventManager.DTOs.Events;

namespace EventsManager.Tests.Events.Delete
{
    public partial class DeleteEventsTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Deleting(NewEventDto newEvent)
        {
            Guid id = (await _eventsService.AddNew(newEvent)).Value;

            var result1 = await _eventsService.Delete(id);
            var result2 = await _eventsService.Delete(id);

            Assert.True(result1.IsSuccess);
            Assert.False(result2.IsSuccess);
        }
    }
}
