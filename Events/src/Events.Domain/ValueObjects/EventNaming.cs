using Shared.Failures.Errors;
using Shared.Validation;
using System.Globalization;

namespace Events.Domain.ValueObjects
{
    public record EventNaming : IValidatableValueObject
    {
        public string Title { get; init; }

        public string Description { get; init; }

        public EventNaming(string title, string description = "")
        {
            Title = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(title); ;

            Description = description;
        }

        public ErrorsCollection Validate()
        {
            ErrorsCollection errors = new();

            if (string.IsNullOrWhiteSpace(Title))
                errors.Add(new Error("Title can't be empty!"));

            else if (Title.Length < 3)
                errors.Add(new Error("Title can't be shorter than 3 symbols!"));

            return errors;
        }
    }
}
