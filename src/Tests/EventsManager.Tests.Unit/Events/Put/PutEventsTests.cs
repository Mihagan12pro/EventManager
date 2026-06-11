using EventManager.Application.Handlers;
using EventManager.Application.Handlers.Events.AddEvent;
using EventManager.Application.Handlers.Events.GetByIdEvent;
using EventManager.Application.Handlers.Events.PutEvent;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.NotFound;
using EventManager.DTOs.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Unit.Events.Put
{
    public partial class PutEventsTests
    {
        [Theory]
        [MemberData(nameof(PutDataForBadRequest))]
        public async Task Test_PuttingBadRequest(
            DateTime start,
            DateTime end)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);

            var provider = TestingServicesProvider.GetServicesProvider();
            
            var addingHandler = provider.GetRequiredService<ICommandHandler<Guid, AddEventCommand>>();
            var puttingHandler = provider.GetRequiredService<ICommandHandler<string, PutEventCommand>>();

            Guid id = await addingHandler.HandleAsync(
                 new AddEventCommand(new NewEventDto(
                     "Юбилей",
                     dateTime.AddDays(1),
                     dateTime.AddDays(2),
                     10)),
                 cts.Token
            );

            PutEventDto putEventDto = new PutEventDto(
                string.Empty,
                start,
                end
            );

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => puttingHandler.HandleAsync(new PutEventCommand(id, putEventDto), cts.Token));
        }

        [Fact]
        public async Task Test_PuttingWithError404()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var provider = TestingServicesProvider.GetServicesProvider();
            var puttingHandler = provider.GetRequiredService<ICommandHandler<string, PutEventCommand>>();

            Guid id = Guid.NewGuid();

            PutEventDto eventDto = new PutEventDto(
                "Birthday",
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2)
            );

            var result = await Assert.ThrowsAsync<NotFoundException>(() => puttingHandler.HandleAsync(new PutEventCommand(id, eventDto), cts.Token));
        }

        [Theory]
        [MemberData(nameof(PutData))]
        public async Task Test_Putting(NewEventDto eventDto)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var provider = TestingServicesProvider.GetServicesProvider();

            var addingHandler = provider.GetRequiredService<ICommandHandler<Guid, AddEventCommand>>();
            var gettingHandler = provider.GetRequiredService<ICommandHandler<GetEventDto, GetByIdEventCommand>>();
            var puttingHandler = provider.GetRequiredService<ICommandHandler<string, PutEventCommand>>();

            DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);

            var id = await addingHandler.HandleAsync(new AddEventCommand(eventDto), cts.Token);

            var oldModel = await gettingHandler.HandleAsync(new GetByIdEventCommand(id), cts.Token);

            await puttingHandler.HandleAsync(
                new PutEventCommand(
                    id, 
                    new PutEventDto(
                        eventDto.Title, eventDto.StartAt, dateTime.AddYears(100))), cts.Token);

            var updatedModel = await gettingHandler.HandleAsync(new GetByIdEventCommand(id), cts.Token);

            Assert.NotEqual(oldModel, updatedModel);
        }
    }
}
