using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using EventManager.Services.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Events.Delete
{
    public partial class DeleteEventsTests
    {
        [Theory]
        [MemberData(nameof(AddEventsForDeleting))]
        [Trait("SubCategory", "Delete")]
        public async Task Test_Basic_Deleting(NewEventDto eventDto)
        {
            var cto = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            var eventsService = provider.GetRequiredService<IEventsService>();

            Guid id = await eventsService.AddNewAsync(eventDto, cto.Token);

            var test1 = await eventsService.DeleteAsync(id, cto.Token);
           
            Assert.Equal(typeof(string), test1.GetType());

            var test2 = await Assert.ThrowsAsync<NotFoundException>(() => eventsService.DeleteAsync(id, cto.Token));
        }

        [Theory]
        [MemberData(nameof(AddNotExistsDeleting))]
        [Trait("SubCategory", "Delete")]
        public async Task Test_Not_Exists_Deleting(Guid id)
        {
            var cto = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            var eventsService = provider.GetRequiredService<IEventsService>();

            var test = await Assert.ThrowsAsync<NotFoundException>(() => eventsService.DeleteAsync(id, cto.Token));
        }
    }
}
