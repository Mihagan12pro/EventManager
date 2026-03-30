using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;

namespace EventsManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Booking_Pending_Status(NewEventDto eventDto)
        {
            IEventsService eventsService = (IEventsService)Activator.CreateInstance(_eventsServiceType);
            IBookingsService bookingsService = (IBookingsService)Activator.CreateInstance(_bookingsServiceType, eventsService);

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            BookingAcceptedDto acceptedDto = await bookingsService.CreateBookingAsync(eventId);

            var booking = await bookingsService.GetBookingByIdAsync(acceptedDto.Id);

            Assert.Equal(BookingStatus.Pending, booking.Status);
        }
    }
}
