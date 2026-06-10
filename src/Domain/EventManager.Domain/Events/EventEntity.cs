using EventManager.Domain.Bookings;
using EventManager.Domain.ValueObjects;
using EventManager.Domain.ValueObjects.DateAndTime;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventManager.Domain.Events
{
    public class EventEntity
    {
        public Guid Id { get; private set; }

        public DateTime StartAt { get; private set; }

        public DateTime EndAt { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public int AvailableSeats { get; private set; }

        public int TotalSeats { get; private set; }



        [NotMapped]
        public required EventNaming EventNamimg
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

                AvailableSeats = _seats.Available;
                TotalSeats = _seats.Total;
            }
        }

        [JsonIgnore]
        public List<BookingModel> Bookings { get; set; } = null!;


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
            lock(_lock)
            {
                Seats = new Seats(TotalSeats, AvailableSeats - count);
            }
        }

        public void ModifyStartAt(DateTime start)
        {
            EventDateTime = new EventDateTime(start, EndAt);
        }

        public void ModifyEndAt(DateTime end)
        {
            EventDateTime = new EventDateTime(StartAt, end);
        }

        public void ModifyBothDatetimes(DateTime start, DateTime end)
        {
            EventDateTime = new EventDateTime(start, end);
        }

        public void ModifyNaming(string title, string description = "")
        {
            EventNamimg = new EventNaming(title, description);
        }
    }
}
