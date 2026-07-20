using Events.Domain.ValueObjects;
using Shared.Failures.Errors;
using Shared.Failures.Exceptions.WebApi.ClientErrors;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Domain
{
    public class Event
    {
        public Guid Id { get; set; }

        public DateTime StartAt { get; private set; }

        public DateTime EndAt { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public int AvailableSeats { get; private set; }

        public int TotalSeats { get; private set; }

        [NotMapped]
        public required EventNaming EventNaming
        {
            get
            {
                return _eventNamimg;
            }
            set
            {
                _eventNamimg = value;

                Title = _eventNamimg.Title;
                Description = _eventNamimg.Description;
            }
        }

        [NotMapped]
        public required EventDateTime EventDateTime
        {
            get
            {
                return _eventDateTime;
            }
            set
            {
                _eventDateTime = value;

                StartAt = _eventDateTime.StartAt;
                EndAt = _eventDateTime.EndAt;
            }
        }

        [NotMapped]
        public required Seats Seats
        {
            get
            {
                return _seats;
            }
            set
            {
                _seats = value;

                TotalSeats = _seats.Total;
                AvailableSeats = _seats.Available;
            }
        }

        private readonly Lock _lock = new Lock();

        private EventDateTime _eventDateTime;
        private Seats _seats;
        private EventNaming _eventNamimg;


        public void ReverseSeats(int count = 1)
        {
            lock (_lock)
            {
                Seats = new Seats(TotalSeats, AvailableSeats - count);
            }
        }

        public void ReleaseSeats(int count = 1)
        {
            lock (_lock)
            {
                Seats = new Seats(TotalSeats, AvailableSeats + count);
            }
        }

        public void Validate()
        {
            ErrorsCollection errors = new ErrorsCollection();

            if (Seats == null)
                Seats = new Seats(TotalSeats, AvailableSeats);

            if (EventNaming == null)
                EventNaming = new EventNaming(Title, Description);

            if (EventDateTime == null)
                EventDateTime = new EventDateTime(StartAt, EndAt);

            errors.AddRange(
                Seats.Validate(),

                EventNaming.Validate(),

                EventDateTime.Validate()
            );

            if (errors.HasErrors)
                throw new BadRequestException(errors);
        }
    }
}
