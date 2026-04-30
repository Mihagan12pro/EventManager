using EventManager.Domain.Bookings;
using System.ComponentModel.DataAnnotations;

namespace EventManager.Domain.Events
{
    public class Event
    {
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public required DateTime StartAt { get; set; }

        public required DateTime EndAt { get; set; }

        public required int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        public List<Booking> Bookings { get; set; }


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
