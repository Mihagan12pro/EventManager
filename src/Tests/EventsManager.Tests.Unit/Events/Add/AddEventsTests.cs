using EventManager.DTOs.Events;
using EventManager.Handlers;
using EventManager.Handlers.Events.AddEvent;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Unit.Events.Add
{
    public partial class AddEventsTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "Add")]
        public async Task Test_Successful_Adding(NewEventDto newEventDto)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var provider = TestingServicesProvider.GetServicesProvider();

            var handler = provider.GetRequiredService<ICommandHandler<Guid, AddEventCommand>>();

            var result = await handler.HandleAsync(new AddEventCommand(newEventDto), cts.Token);

            Assert.Equal(typeof(Guid), result.GetType());
        }

        [Theory]
        [MemberData(nameof(AddBadRequest))]
        [Trait("SubCategory", "Add")]
        public async Task Test_Bad_Request(NewEventDto dto, int expected)
        {
            IServiceProvider serviceProvider = TestingServicesProvider.GetServicesProvider();

            CancellationTokenSource cts = new CancellationTokenSource();
            var handler = serviceProvider.GetRequiredService<ICommandHandler<Guid, AddEventCommand>>();

            var result = await Assert.ThrowsAsync<BadRequestException>(() => handler.HandleAsync(new AddEventCommand(dto), cts.Token));

            Assert.Equal(expected, result.Error.Errors.Count());
        }
    }
}
