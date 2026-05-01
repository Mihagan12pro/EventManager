using EventManager.Domain.Bookings.Enums;
using EventManager.Domain.Events;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.Conflict;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventManager.Tests.Booking.Background
{
    public partial class BackgroundBookingHandlingTests
    {
        [Fact]
        [Trait("SubCategory", "BackgroundHandling")]
        public async Task Test_Rejected()
        {
            var provider = GetProviderService();

            var hostedServices = provider.GetServices<IHostedService>();
            var cancellarationTokenSource = new CancellationTokenSource();

            var startTasks = hostedServices.Select(s => s.StartAsync(cancellarationTokenSource.Token));
            await Task.WhenAll(startTasks);

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            var eventDto = new NewEventDto(
                "Хакатон",
                DateTime.Now.AddMonths(5),
                DateTime.Now.AddMonths(5).AddHours(10),
                5);

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            BookingAcceptedDto? bookingDto = null;

            Task task1 = Task.Run(async () => { await Task.Delay(200); await eventsService.DeleteAsync(eventId); });
            Task task2 = Task.Run( async () => bookingDto = await bookingsService.CreateBookingAsync(eventId));


            await Task.WhenAll(task1, task2);
            
            await Task.Delay(3000);

            var booking = await bookingsService.GetBookingByIdAsync(bookingDto.Id);

            Assert.Equal(BookingStatus.Rejected, booking.Status);
        }

        [Fact]
        [Trait("SubCategory", "BackgroundHandling")]
        public async Task Test_NoAvaliableSeats()
        {
            var provider = GetProviderService();

            var hostedServices = provider.GetServices<IHostedService>();
            var cancellarationTokenSource = new CancellationTokenSource();

            var startTasks = hostedServices.Select(s => s.StartAsync(cancellarationTokenSource.Token));
            await Task.WhenAll(startTasks);

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            var eventDto = new NewEventDto(
                "Хакатон",
                DateTime.Now.AddMonths(5), 
                DateTime.Now.AddMonths(5).AddHours(10),
                5);

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            ParallelOptions options = new ParallelOptions() { MaxDegreeOfParallelism = 20 };

            await Parallel.ForAsync(0, 19, options, async (i, token) =>
                {
                    try
                    {
                        await bookingsService.CreateBookingAsync(eventId);
                    }
                    catch(Exception ex)
                    {
                        Assert.Equal(typeof(NoAvailableSeatsException), ex.GetType());
                    }
                });

            var bookings = await bookingsService.GetAllAsync(new BookingFiltersDto(null, null, null));

            await Task.Delay(3000);

            var confirmed = bookings.Where(b => b.Status == BookingStatus.Confirmed);

            Assert.Equal(5, confirmed.Count());
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "BackgroundHandling")]
        public async Task Test_UniqueBookingId(NewEventDto eventDto)
        {
            var provider = GetProviderService();

            var hostedServices = provider.GetServices<IHostedService>();
            var cancellarationTokenSource = new CancellationTokenSource();

            var startTasks = hostedServices.Select(s => s.StartAsync(cancellarationTokenSource.Token));
            await Task.WhenAll(startTasks);

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            HashSet<Guid> bookingsIds = new HashSet<Guid>();

            int totalSeats = eventDto.TotalSeats!.Value;

            for(int i = 0; i < totalSeats; i++)
            {
                var bookingAccepted = await bookingsService.CreateBookingAsync(eventId);
                bookingsIds.Add(bookingAccepted.Id);
            }

            EventModel @event = await eventsService.GetEventByIdAsync(eventId);

            Assert.Equal(0, @event.AvailableSeats);
            Assert.Equal(@event.TotalSeats, bookingsIds.Count);
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "BackgroundHandling")]
        public async Task Test_BackgroundService(NewEventDto eventDto)
        {
            var provider = GetProviderService();

            var hostedServices = provider.GetServices<IHostedService>();
            var cancellarationTokenSource = new CancellationTokenSource();

            var startTasks = hostedServices.Select(s => s.StartAsync(cancellarationTokenSource.Token));
            await Task.WhenAll(startTasks);

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            BookingAcceptedDto accepted1 = await bookingsService.CreateBookingAsync(eventId);
            BookingAcceptedDto accepted2 = await bookingsService.CreateBookingAsync(eventId);
            BookingAcceptedDto accepted3 = await bookingsService.CreateBookingAsync(eventId);
            BookingAcceptedDto accepted4 = await bookingsService.CreateBookingAsync(eventId);

            var booking1Pending = await bookingsService.GetBookingByIdAsync(accepted1.Id);
            var booking2Pending = await bookingsService.GetBookingByIdAsync(accepted2.Id);
            var booking3Pending = await bookingsService.GetBookingByIdAsync(accepted3.Id);
            var booking4Pending = await bookingsService.GetBookingByIdAsync(accepted4.Id);

            Assert.Equal(BookingStatus.Pending, booking1Pending.Status);
            Assert.Equal(BookingStatus.Pending, booking2Pending.Status);
            Assert.Equal(BookingStatus.Pending, booking3Pending.Status);
            Assert.Equal(BookingStatus.Pending, booking4Pending.Status);

            await Task.Delay(9000);

            var booking1Confirmed = await bookingsService.GetBookingByIdAsync(accepted1.Id);

            var booking2Confirmed = await bookingsService.GetBookingByIdAsync(accepted2.Id);

            var booking3Confirmed = await bookingsService.GetBookingByIdAsync(accepted3.Id);

            var booking4Confirmed = await bookingsService.GetBookingByIdAsync(accepted4.Id);

            Assert.True(booking1Confirmed.Status == BookingStatus.Rejected
                || booking1Confirmed.Status == BookingStatus.Confirmed);

            Assert.True(booking2Confirmed.Status == BookingStatus.Rejected
                || booking2Confirmed.Status == BookingStatus.Confirmed);

            Assert.True(booking3Confirmed.Status == BookingStatus.Rejected ||
                booking3Confirmed.Status == BookingStatus.Confirmed);

            Assert.True(booking4Confirmed.Status == BookingStatus.Rejected ||
                booking4Confirmed.Status == BookingStatus.Confirmed);
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "BackgroundHandling")]
        public async Task Test_BookingPendingStatus(NewEventDto eventDto)
        {
            var provider = GetProviderService();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            BookingAcceptedDto acceptedDto = await bookingsService.CreateBookingAsync(eventId);

            var booking = await bookingsService.GetBookingByIdAsync(acceptedDto.Id);

            Assert.Equal(BookingStatus.Pending, booking.Status);
        }
    }
}
