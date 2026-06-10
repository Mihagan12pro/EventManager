using EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.Conflict;

namespace EventManager.Domain.ValueObjects
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
                    throw new BadRequestException("There are no avaliable seats!");
                
                _available = value;
            }
        }

        public Seats(int total)
        {
            Total = total;

            if (total <= 0)
            {
                throw new ConflictException("Total count must be greater than zero!");
            }

            Available = total;
        }

        public Seats(int total, int avaliable)
        {
            Total = total;

            if (total <= 0)
            {
                throw new ConflictException("Total count must be greater than zero!");
            }
        }


        private int _total, _available;
    }
}
