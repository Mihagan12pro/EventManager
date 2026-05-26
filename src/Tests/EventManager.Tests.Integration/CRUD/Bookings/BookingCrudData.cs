using EventManager.DataAccess.PostgreSQL.DbContexts;
using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Integration.CRUD.Bookings
{
    public partial class BookingCrudTests
    {
        private static readonly DateTime now = DateTime.UtcNow;

        private async Task Seed()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 5);
            var provider = await GetServiceProviderAsync();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();
            var bookingsRepository = provider.GetRequiredService<IBookingsRepository>();

            Guid eventId = await eventsRepository.AddNewAsync(newEvent, cts.Token);

            DateTime dateTime = now.AddDays(1);

            for(int i = 0; i < newEvent.TotalSeats; i++)
            {
                await bookingsRepository.CreateNewBookingAsync(eventId, cts.Token);
            }

            var dbContext = provider.GetRequiredService<AppDbContextBase>();

            var bookings = dbContext.Bookings
                .ToList();

            var booking1 = bookings[0];
            booking1.Status = BookingStatus.Rejected;
            booking1.CreatedAt = now;
            booking1.ProcessedAt = now.AddDays(7);

            var booking2 = bookings[1];
            booking2.Status = BookingStatus.Confirmed;
            booking2.CreatedAt = now;
            booking2.ProcessedAt = now.AddDays(5);

            var booking3 = bookings[2];
            booking3.Status = BookingStatus.Confirmed;
            booking3.CreatedAt = now;
            booking3.ProcessedAt = now.AddDays(5);

            var booking4 = bookings[3];
            booking4.Status = BookingStatus.Confirmed;
            booking4.CreatedAt = now;
            booking4.ProcessedAt = now;
                
            var booking5 = bookings[4];
            booking5.CreatedAt = now.AddMinutes(10);

            await dbContext.SaveChangesAsync(cts.Token);
        }

        public static IEnumerable<object[]> FilterByDto()
        {
            return
            [
                [
                    new BookingFiltersDto(BookingStatus.Confirmed, null, null),

                    3
                ],

                [
                    new BookingFiltersDto(BookingStatus.Pending, null, null),

                    1
                ],

                [
                    new BookingFiltersDto(BookingStatus.Rejected, null, null),

                    1
                ],

                [
                    new BookingFiltersDto(null, now, null),

                    4
                ],

                [
                    new BookingFiltersDto(BookingStatus.Pending, now, null),

                    0
                ],

                [
                    new BookingFiltersDto(BookingStatus.Confirmed, now, null),

                    3
                ],

                [
                    new BookingFiltersDto(BookingStatus.Confirmed, now, now),

                    1
                ],

                [
                    new BookingFiltersDto(BookingStatus.Confirmed, now, now.AddDays(5)),

                    2
                ],
            ];
        }
    }
}
