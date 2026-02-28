namespace EventManager.DomainModels
{
    public class Event
    {
        public required Guid Id { get; set; }

        public required string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public required DateTime StartAt { get; set; }

        public required DateTime EndAt { get; set; }
    }
}
