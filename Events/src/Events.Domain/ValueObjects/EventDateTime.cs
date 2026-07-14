using Shared.Failures.Errors;
using Shared.Validation;

namespace Events.Domain.ValueObjects
{
    public record EventDateTime : IValidatableValueObject
    {
        private Dictionary<string, Error> _propertyError = new();

        private DateTime _startAt, _endAt;

        public DateTime StartAt
        {
            get
            {
                return _startAt;
            }
            init
            {
                if (value <= DateTime.Now)
                    _propertyError[nameof(StartAt)] = new Error("Too late for start date time!");
                else
                {
                    if (_propertyError.ContainsKey(nameof(StartAt)))
                        _propertyError.Remove(nameof(StartAt));
                }

                _startAt = value;
            }
        }

        public DateTime EndAt
        {
            get
            {
                return _endAt;
            }
            init
            {
                if (value <= StartAt)
                    _propertyError[nameof(EndAt)] = new Error("End date time must be later than start date time!");
                else
                {
                    if (_propertyError.ContainsKey(nameof(EndAt)))
                        _propertyError.Remove(nameof(EndAt));
                }

                _endAt = value;
            }
        }

        public ErrorsCollection Validate()
        {
            ErrorsCollection errors = new();

            foreach (var property in _propertyError.Keys)
            {
                errors.Add(_propertyError[property]);
            }

            return errors;
        }

        public EventDateTime Update(
            DateTime? updatedStartAt, 
            DateTime? updatedEndAt)
        {
            DateTime startAt = StartAt;

            DateTime endAt = EndAt;

            if (updatedEndAt != null)
                endAt = updatedEndAt.Value;

            if (updatedStartAt != null)
                startAt = updatedStartAt.Value;

            return new EventDateTime(startAt, endAt);
        }

        public EventDateTime(DateTime start, DateTime end)
        {
            StartAt = start;

            EndAt = end;
        }
    }
}
