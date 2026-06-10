using EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest;

namespace EventManager.Domain.ValueObjects.DateAndTime
{
    public record EventDateTime
    {
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
                    throw new BadRequestException("Too late date for creating new events!");

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
                    throw new BadRequestException("End date time must be later than start date time!");

                _endAt = value;
            }
        }

        public EventDateTime(DateTime start, DateTime end)
        {
            StartAt = start;
            EndAt = end;
        }
    }
}
