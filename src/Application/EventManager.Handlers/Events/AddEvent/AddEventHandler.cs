using EventManager.DTOs.Events;
using EventManager.Handlers.Extensions.Validation;
using EventManager.Repositories.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventsManager.Failures.Errors.Collections;
using FluentValidation;

namespace EventManager.Handlers.Events.AddEvent
{
    public class AddEventHandler : ICommandHandler<Guid, AddEventCommand>
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IValidator<NewEventDto> _validator;

        public async Task<Guid> HandleAsync(
            AddEventCommand command,
            CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(command.NewEvent);

            if (!validation.IsValid)
            {
                ErrorsCollection errors = new ErrorsCollection(validation.Errors.Select(vf => vf.ToError()));

                throw new BadRequestException(errors);
            }

            Guid id = await _eventsRepository.AddNewAsync(command.NewEvent, cancellationToken);

            return id;
        }

        public AddEventHandler(
            IEventsRepository eventsRepository,
            IValidator<NewEventDto> validator)
        {
            _eventsRepository = eventsRepository;
            _validator = validator;
        }
    }
}
