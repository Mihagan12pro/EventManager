using EventManager.Domain.Events;
using EventManager.DTOs.Events;

namespace EventManager.Shared.Filters
{
    public class EventsFilters : Filters<EventEntity>
    {
        public void Add(GetEventsWithFiltersDto eventsDto)
        {
            Add((EventEntity e) => e.Title.StartsWith(eventsDto.Title), () => eventsDto.Title != null);
            Add((EventEntity e) => e.StartAt == eventsDto.DateRange.LowerBound, () => eventsDto.DateRange.LowerBound != null);
            Add((EventEntity e) => e.EndAt == eventsDto.DateRange.UpperBound, () => eventsDto.DateRange.UpperBound != null);
        }
    }
}
