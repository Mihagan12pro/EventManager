using Shared.Failures.Errors;

namespace Shared.Validation
{
    public interface IValidatableValueObject
    {
        ErrorsCollection Validate();
    }
}
