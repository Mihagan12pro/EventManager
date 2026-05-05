using EventManager.DTOs.Events;
using FluentValidation;

namespace EventManager.Services.Events.Validators
{
    public class PutEventValidator : AbstractValidator<PutEventDto>
    {
        public PutEventValidator()
        {
            RuleFor(e => e.Title)
                .MinimumLength(1)
                    .WithMessage("Title's length must be greater than zero!");

            RuleFor(e => e.EndAt)
                .GreaterThan(e => e.StartAt)
                    .WithMessage("End date time must be greater than star date time!")
                        .NotNull()
                            .WithMessage("End date is a required field!");

            RuleFor(e => e.StartAt - DateTime.Now)
                .GreaterThanOrEqualTo(TimeSpan.FromDays(1))
                    .WithMessage("Too late for creating new events!")
                        .NotNull()
                            .WithMessage("Start date is a required field!");
        }
    }
}
