using EventManager.Domain.Bookings;
using EventManager.Domain.Failures.Errors;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest;
using EventManager.Domain.ValueObjects.Events;
using EventManager.Domain.ValueObjects.Events.DateAndTime;
using EventsManager.Failures.Errors;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
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

                TotalSeats = _seats.Total;
                AvailableSeats = _seats.Available;
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
            lock (_lock)
            {
                Seats = new Seats(TotalSeats, AvailableSeats + count);
            }
        }

        public void ModifyDatetimes(DateTime start, DateTime end)
        {
            EventDateTime = new EventDateTime(start, end);
        }

        public void ModifyNaming(string title, string description = "")
        {
            EventNamimg = new EventNaming(title, description);
        }
    }
}
