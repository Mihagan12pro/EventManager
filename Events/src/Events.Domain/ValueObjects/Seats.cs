using Events.Domain.Exceptions;
using Shared.Failures.Errors;
using Shared.Failures.Exceptions.WebApi.ClientErrors;
using Shared.Validation;

namespace Events.Domain.ValueObjects
{
    public record Seats : IValidatableValueObject
    {
        public int Total
        {
            get
            {
                return _total;
            }
            init
            {
                _total = value;
            }
        }

        public int Available
        {
            get
            {
                return _available;
            }
            init
            {
                if (value > Total)
                    throw new ConflictException("Count of avaliable seats can't be greater than count of total seats!");

                if (value < 0)
                    throw new NoAvailableSeatsException("There are no avaliable seats!");

                _available = value;
            }
        }

        public Seats(int total)
        {
            Total = total;

            Available = total;
        }

        public Seats(int total, int avaliable)
        {
            Total = total;

            Available = avaliable;
        }


        private int _total, _available;

        public ErrorsCollection Validate()
        {
            ErrorsCollection errors = new();

            if (Total < 1)
                errors.Add(new Error("Total count of seats must be greater than zero!"));

            return errors;
        }
    }
}
