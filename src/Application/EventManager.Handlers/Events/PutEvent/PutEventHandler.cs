using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.Handlers.Extensions;
using EventManager.Repositories.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using EventsManager.Failures.Errors.Collections;
using FluentValidation;

namespace EventManager.Handlers.Events.PutEvent
{
    public class PutEventHandler : ICommandHandler<string, PutEventCommand>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IValidator<PutEventDto> _validator;

        public async Task<string> HandleAsync(
            PutEventCommand command, 
            CancellationToken cancellationToken)
        {
            Guid id = command.Id;
            PutEventDto putEvent = command.PutEvent;


            EventModel? eventById = await _eventsRepository.GetByIdAsync(id, cancellationToken);

            if (eventById == null)
                throw new NotFoundException($"Event with id = '{id}' was not found!");

            var validation = _validator.Validate(putEvent);

            if (!validation.IsValid)
            {
                ErrorsCollection errors = new ErrorsCollection(validation.Errors.Select(vf => vf.ToError()));

                throw new BadRequestException(errors);
            }

            await _eventsRepository.CompleteUpdateAsync(id, putEvent, cancellationToken);

            return $"Event with id = {id} had been updated!";
        }

        public PutEventHandler(
            IEventsRepository eventsRepository,
            IValidator<PutEventDto> validator)
        {
            _eventsRepository = eventsRepository;
            _validator = validator;
        }
    }
}
