using EventManager.DTOs.Events;
using EventManager.Services.Events;

namespace EventsManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Booking_Pending_Status(NewEventDto eventDto)
        {
            EventsService eventsService = new EventsService();
         

            Guid eventId = await eventsService.AddNewAsync(eventDto);
        }
    }
}
