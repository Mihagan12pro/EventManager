using EventManager.DTOs.Events;
using EventManager.Services.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Services.Tests.Background.BookingHandling
{
    public partial class BookingHandlingTests
    {
        [Fact]
        [Trait("SubCategory", "BookingHandling")]
        public async Task Test_Rejected()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var provider = TestingServicesProvider.GetServicesProvider();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();

            Guid eventId = await eventsService.AddNewAsync(
                new NewEventDto(
                    "Birthday", 
                    DateTime.Now.AddDays(10), 
                    DateTime.Now.AddDays(11), 
                    10), 
                cts.Token);


            await Task.Delay(10000);
        }
    }
}
