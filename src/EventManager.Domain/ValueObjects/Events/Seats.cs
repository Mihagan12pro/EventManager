using EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.Conflict;
using EventsManager.Failures.Errors;

namespace EventManager.Domain.ValueObjects.Events
{
    public record Seats
    {
        public int Total
        {
            get
            {
                return _total;
            }
            init
            {
                if (value <= 0)
                    throw new BadRequestException("Total count of seats must be greater than zero!");

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
    }
}
