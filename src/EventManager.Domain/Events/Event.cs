using EventManager.Domain.Bookings;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventManager.Domain.Events
{
    public class EventModel
    {
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public required DateTime StartAt { get; set; }

        public required DateTime EndAt { get; set; }

        public required int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        [JsonIgnore]
        public List<BookingModel> Bookings { get; set; } = null!;


        private readonly Lock _lock = new Lock();

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
