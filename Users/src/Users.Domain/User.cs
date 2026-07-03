using Shared.Failures.Errors;
using Shared.Failures.Exceptions.WebApi.ClientErrors;
using Shared.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Users.Domain.Enums;
using Users.Domain.ValueObjects;

namespace Users.Domain
{
    public class UserEntity : IValidatableEntity
    {
        public Guid Id { get; private set; }

        [NotMapped]
        [JsonIgnore]
        public required UserName UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;

                Login = _userName.Name;
            }
        }

        [MinLength(3)]
        [MaxLength(256)]
        public string Login { get; private set; }

        public required Roles Role { get; set; }

        public required string HashedPassword { get; set; }
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

        public static void ValidateActiveBookings(int count)
        {
            if (count >= MaxActiveBookings)
                throw new ConflictException("User can't has more than 10 booking!");
        }

        public const int MaxActiveBookings = 10;

        private UserName _userName;
    }
}
