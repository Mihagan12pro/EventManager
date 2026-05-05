using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventManager.Services.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Events.Add
{
    public partial class AddEventsTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "Add")]
        public async Task Test_Successful_Adding(NewEventDto newEventDto)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var provider = TestingServicesProvider.GetProviderService();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();

            var result = await eventsService.AddNewAsync(newEventDto, cts.Token);
            var deletingResult = await eventsService.DeleteAsync(result, cts.Token);

            Assert.Equal(typeof(Guid), result.GetType());
            Assert.Equal(typeof(string), deletingResult.GetType());
        }

        [Theory]
        [MemberData(nameof(AddBadRequest))]
        [Trait("SubCategory", "Add")]
        public async Task Test_Bad_Request(NewEventDto dto, int expected)
        {
            IServiceProvider serviceProvider = TestingServicesProvider.GetProviderService();

            CancellationTokenSource cts = new CancellationTokenSource();
            IEventsService eventsService = serviceProvider.GetRequiredService<IEventsService>();

            var result = await Assert.ThrowsAsync<BadRequestException>(() => eventsService.AddNewAsync(dto, cts.Token));

            Assert.Equal(expected, result.Error.Errors.Count());
        }
    }
}
