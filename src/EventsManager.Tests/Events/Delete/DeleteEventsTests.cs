using EventManager.DTOs.Events;
using EventManager.Services.Events;

namespace EventsManager.Tests.Events.Delete
{
    [Collection("Delete events collection")]
    public partial class DeleteEventsTests
    {
        [Theory]
        [MemberData(nameof(AddEventsForDeleting))]
        public async Task Test_Basic_Deleting(NewEventDto eventDto)
        {
            var eventsService = new EventsService();

            Guid id = (await eventsService.AddNew(eventDto)).Value;

            var test1 = await eventsService.Delete(id);
            var test2 = await eventsService.Delete(id);

            Assert.True(test1.IsSuccess);
            Assert.False(test2.IsSuccess);
        }

        [Theory]
        [MemberData(nameof(AddNotExistsEventsForDeleting))]
        public async Task Test_Not_Exists_Deleting(Guid id)
        {
            var eventsService = new EventsService();

            var test = await eventsService.Delete(id);

            Assert.False(test.IsSuccess);
        }
    }
}
