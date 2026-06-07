using EventManager.DTOs.Shared;
using FluentValidation;

namespace EventManager.Handlers.CommonValidators
{
    public class PaginationDtoValidator : AbstractValidator<PaginationDto>
    {
        public PaginationDtoValidator()
        {
            RuleFor(p => p.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than zero!");

            RuleFor(p => p.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than zero!");
        }
    }
}
