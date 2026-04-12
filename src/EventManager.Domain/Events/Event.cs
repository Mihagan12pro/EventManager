using System.ComponentModel.DataAnnotations;

namespace EventManager.Domain.Events
{
    public class Event
    {
        public required Guid Id { get; set; }

        public required string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public required DateTime StartAt { get; set; }

        public required DateTime EndAt { get; set; }

        public required int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        private readonly Lock _lock = new Lock();

        public bool TryReverseSeats([Range(1, int.MaxValue)] int count = 1)
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

        public void ReleaseSeats(int count = 1)
        {
            throw new NotImplementedException();
        }
    }
}
