using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Services.Exceptions;

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

            Guid id = await eventsService.AddNewAsync(eventDto);

            var test1 = await eventsService.DeleteAsync(id);
           
            Assert.Equal(typeof(string), test1.GetType());

            var test2 = await Assert.ThrowsAsync<NotFoundException>(() => eventsService.DeleteAsync(id));
        }

        [Theory]
        [MemberData(nameof(AddNotExistsDeleting))]
        public async Task Test_Not_Exists_Deleting(Guid id)
        {
            var eventsService = new EventsService();

            var test = await Assert.ThrowsAsync<NotFoundException>(() => eventsService.DeleteAsync(id));
        }
    }
}
