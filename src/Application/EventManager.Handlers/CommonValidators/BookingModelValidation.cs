using EventManager.Domain.Bookings;
using FluentValidation;

namespace EventManager.Handlers.CommonValidators
{
    public class BookingModelValidation : AbstractValidator<BookingModel>
    {
        public BookingModelValidation()
        {
            RuleFor(b => b)
                .NotNull();
        }
    }
}
