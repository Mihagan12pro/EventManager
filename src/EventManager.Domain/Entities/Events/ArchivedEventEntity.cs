using System.Text.Json.Serialization;

namespace EventManager.Domain.Entities.Events
{
    public class ArchivedEventEntity
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        [JsonIgnore]
        public EventEntity Event { get; set; } = null!;
    }
}
