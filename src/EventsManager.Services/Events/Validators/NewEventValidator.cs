using EventManager.DTOs.Events;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Services.Events.Validators
{
    internal class NewEventValidator : AbstractValidator<NewEventDto>
    {
        public NewEventValidator()
        {
            RuleFor(e => e.EndAt).GreaterThan(e => e.StartAt).WithMessage("End date time must be greater than star date time!");

            RuleFor(e => e.TotalSeats).GreaterThan(0).WithMessage("Count of total seats must be greater than zero!");
        }
    }
}
