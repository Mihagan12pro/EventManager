using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace EventManager.Tests.Integration.Bookings
{
    public class BookingsRepositoryTests : RepositoryTestBase
    {
        public BookingsRepositoryTests(EventManagerAppFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Test_PendingToComfirmed()
        {
            await ResetDatabaseAsync();
            var provider = await GetServiceProviderAsync();
            CancellationTokenSource cts = new CancellationTokenSource();

            var hostedServices = provider.GetServices<IHostedService>();
            var startTasks = hostedServices.Select(s => s.StartAsync(cts.Token));
            await Task.WhenAll(startTasks);

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();
            var bookingsRepository = provider.GetRequiredService<IBookingsRepository>();

            NewEventDto newEventDto = new NewEventDto("Birthday", DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(1).AddHours(20), 10, "Daddy's birthday");

            Guid eventId = await eventsRepository.AddNewAsync(newEventDto, cts.Token);

            var id = await bookingsRepository.CreateNewBookingAsync(eventId, cts.Token);

            var pending = await bookingsRepository.GetByIdAsync(id, cts.Token);
            Assert.Equal(BookingStatus.Pending, pending.Status);

            await Task.Delay(10000);

            var confirmed = await bookingsRepository.GetByIdAsync(id, cts.Token);
            Assert.Equal(BookingStatus.Confirmed, confirmed.Status);
        }
    }
}
