using EventManager.DTOs.Events;
using FluentValidation;

namespace EventManager.Services.Events.Validators
{
    public class NewEventValidator : AbstractValidator<NewEventDto>
    {
        public NewEventValidator()
        {
            RuleFor(e => e.EndAt)
                .GreaterThan(e => e.StartAt)
                    .WithMessage("End date time must be greater than star date time!");

            RuleFor(e => e.StartAt - DateTime.Now)
                .GreaterThanOrEqualTo(TimeSpan.FromDays(1))
                    .WithMessage("Too late for creating new events!");

            RuleFor(e => e.TotalSeats)
                .GreaterThan(0)
                    .WithMessage("Count of total seats must be greater than zero!");
        }
    }
}
