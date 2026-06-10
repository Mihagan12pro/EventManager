using EventManager.Domain.Bookings;
using EventManager.Domain.ValueObjects.DateAndTime;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventManager.Domain.Events
{
    public class EventModel
    {
        public required int TotalSeats { get; set; }

        public required string Title { get; set; }

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

        public Guid Id { get; set; }

        public DateTime StartAt { get; private set; }

        public DateTime EndAt { get; private set; }

        public string Description { get; set; } = string.Empty;

        public int AvailableSeats { get;  set; }

        [JsonIgnore]
        public List<BookingModel> Bookings { get; set; } = null!;


        private readonly Lock _lock = new Lock();
        private EventDateTime _eventDateTime;

        public bool TryReverseSeats(int count = 1)
        {
            if (count > AvailableSeats)
            {
                return false;
            }

            lock (_lock)
            {
                AvailableSeats -= count;
            }

            return true;
        }

        public bool TryReleaseSeats(int count = 1)
        {
            if (count + AvailableSeats > TotalSeats)
                return false;

            lock(_lock)
            {
                AvailableSeats += count;
            }

            return true;
        }
    }
}
