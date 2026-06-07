using EventManager.Domain.Events;
using FluentValidation;

namespace EventManager.Handlers.CommonValidators
{
    public class EventModelValidator : AbstractValidator<EventModel>
    {
        public EventModelValidator()
        {
            RuleFor(e => e)
                .NotNull();
        }
    }
}
