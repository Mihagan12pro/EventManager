using EventManager.Domain.Failures.Errors;
using EventManager.Domain.Validation;
using EventsManager.Failures.Errors;
using System.ComponentModel.DataAnnotations;

namespace EventManager.Domain.ValueObjects.Users
{
    public record UserName : IValidatableValueObject
    {
        [Length(3, 256)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; init; }

        public ErrorsCollection Validate()
        {
            ErrorsCollection errors = new ErrorsCollection();


            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);

            if (!Validator.TryValidateObject(this, context, results, true))
            {
                foreach (var item in results)
                {
                    errors.Add(new Error(item.ErrorMessage));
                }
            }

            return errors;
        }

        public UserName(string name)
        {
            Name = name;
        }
    }
}
