using EventManager.Domain.Failures.Errors;
using EventsManager.Failures.Errors;

namespace EventManager.Domain.Validation
{
    /// <summary>
    /// Use only for value objects
    /// </summary>
    public interface IValidatableValueObject
    {
        ErrorsCollection Validate();
    }
}
