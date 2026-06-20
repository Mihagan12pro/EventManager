using EventManager.Domain.Entities.Bookings;
using EventManager.Domain.Entities.Users.Enums;
using EventManager.Domain.Failures.Errors;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest;
using EventManager.Domain.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventManager.Domain.Entities.Users
{
    public class UserEntity : IValidatableEntity
    {
        public Guid Id { get; private set; }

        [MinLength(3)]
        [MaxLength(256)]
        public required string Login { get; set; }

        public required string HashedPassword { get; set; }

        public required Roles Role { get; set; }

        [JsonIgnore]
        public List<BookingEntity> Bookings { get; set; } = null!;

        public void Validate()
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);
            
            if (!Validator.TryValidateObject(this, context, results, true))
            {
                ErrorsCollection errors = new ErrorsCollection(results);

                throw new BadRequestException(errors);
            }
        }
    }
}
