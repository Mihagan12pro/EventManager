using EventManager.Domain.Failures.Errors;

namespace EventManager.Domain.ValueObjects
{
    public abstract record ValueObject
    {
        public ErrorsCollection ValidationErrors { get; private set; } = new ErrorsCollection();
    }
}
