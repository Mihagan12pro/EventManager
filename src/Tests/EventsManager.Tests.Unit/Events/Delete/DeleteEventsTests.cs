using EventManager.Application.Handlers;
using EventManager.Application.Handlers.Events.AddEvent;
using EventManager.Application.Handlers.Events.DeleteEvent;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.NotFound;
using EventManager.DTOs.Events;
using EventManager.Handlers;
using EventManager.Handlers.Events.AddEvent;
using EventManager.Handlers.Events.DeleteEvent;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Unit.Events.Delete
{
    public partial class DeleteEventsTests
    {
        [Theory]
        [MemberData(nameof(AddEventsForDeleting))]
        [Trait("SubCategory", "Delete")]
        public async Task Test_BasicDeleting(NewEventDto eventDto)
        {
            var cto = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            var addingHandler = provider.GetRequiredService<ICommandHandler<Guid, AddEventCommand>>();
            var deletingHandler = provider.GetRequiredService<ICommandHandler<string, DeleteEventCommand>>();

            Guid id = await addingHandler.HandleAsync(new AddEventCommand(eventDto), cto.Token);

            var test1 = await deletingHandler.HandleAsync(new DeleteEventCommand(id), cto.Token);
           
            Assert.Equal(typeof(string), test1.GetType());

            var test2 = await Assert.ThrowsAsync<NotFoundException>(() => deletingHandler.HandleAsync(new DeleteEventCommand(id), cto.Token));
        }

        [Theory]
        [MemberData(nameof(AddNotExistsDeleting))]
        [Trait("SubCategory", "Delete")]
        public async Task Test_NotExistsDeleting(Guid id)
        {
            var cto = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            var handler = provider.GetRequiredService<ICommandHandler<string, DeleteEventCommand>>();

            var test = await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(new DeleteEventCommand(id), cto.Token));
        }
    }
}
