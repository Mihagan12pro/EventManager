using EventManager.DTOs.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Integration.CRUD.Bookings
{
    public partial class BookingCrudTests : IntegrationTests
    {
        public BookingCrudTests(EventManagerAppFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Test_CreateNewBookingAsync()
        {
            await ResetDatabaseAsync();
            var provider = await GetServiceProviderAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();
            var bookingsRepository = provider.GetRequiredService<IBookingsRepository>();

            Guid eventId = await eventsRepository.AddNewAsync(
                new NewEventDto(
                    "Friday 13", 
                    DateTime.UtcNow.AddDays(1), 
                    DateTime.UtcNow.AddDays(2), 
                    10
                ), 
               cts.Token
            );

            var accepted = await bookingsRepository.CreateNewBookingAsync(eventId, cts.Token);

            Assert.NotNull(accepted);
        }

        [Fact]
        public async Task Test_GetByIdAsync()
        {
            await ResetDatabaseAsync();
            var provider = await GetServiceProviderAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();
            var bookingsRepository = provider.GetRequiredService<IBookingsRepository>();

            Guid eventId = await eventsRepository.AddNewAsync(
                new NewEventDto(
                    "Friday 13",
                    DateTime.UtcNow.AddDays(1),
                    DateTime.UtcNow.AddDays(2),
                    10
                ),
               cts.Token
            );

            var acceptedId = await bookingsRepository.CreateNewBookingAsync(eventId, cts.Token);

            var bookingModel = await bookingsRepository.GetByIdAsync(acceptedId, cts.Token);

            Assert.NotNull(bookingModel);
        }
    }
}
