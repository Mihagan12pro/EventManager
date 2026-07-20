using Events.Domain;
using Events.Domain.ValueObjects;

namespace Events.Infrastracture.Entities
{
    public class EventEntity
    {
        public Guid Id { get; set; }

        public required DateTime StartAt { get; set; }

        public required DateTime EndAt { get; set; }

        public required string Title { get; set; }

        public required int TotalSeats { get; set; }

        public required int AvailableSeats { get; set; }

        public string Description { get; set; }

        public void Update(Event @event)
        {
            StartAt = @event.StartAt;

            EndAt = @event.EndAt;

            Title = @event.Title;

            Description = @event.Description;

            AvailableSeats = @event.AvailableSeats;
        }

        public static Event ExtractEvent(EventEntity eventEntity)
            => new Event()
                {
                    EventDateTime = new EventDateTime(eventEntity.StartAt, eventEntity.EndAt),

                    Seats = new Seats(eventEntity.TotalSeats, eventEntity.AvailableSeats),

                    EventNaming = new EventNaming(eventEntity.Title, eventEntity.Description),

                    Id = eventEntity.Id,
                };

        public static EventEntity ExtractEntity(Event @event)
            => new EventEntity()
                { 
                    AvailableSeats = @event.AvailableSeats,
                
                    Description = @event.Description, 
                
                    EndAt = @event.EndAt, 
                
                    StartAt = @event.StartAt, 
                
                    Title = @event.Title, 
                
                    TotalSeats = @event.TotalSeats,

                    Id = @event.Id
            };
    }
}
