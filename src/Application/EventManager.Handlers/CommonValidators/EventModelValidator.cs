using EventManager.Domain.Events;
using FluentValidation;

namespace EventManager.Handlers.CommonValidators
{
    public class EventModelValidator : AbstractValidator<EventEntity>
    {
        public EventModelValidator()
        {
            RuleFor(e => e)
                .NotNull();
        }
    }
}
