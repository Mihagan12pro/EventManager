using EventManager.Domain.Events;
using EventManager.DTOs.Events;

namespace EventsManager.Shared.Filters
{
    public class EventsFilters : Filters<EventModel>
    {
        public void Add(GetEventsWithFiltersDto eventsDto)
        {
            Add((EventModel e) => e.Title.StartsWith(eventsDto.Title), () => eventsDto.Title != null);
            Add((EventModel e) => e.StartAt == eventsDto.DateRange.LowerBound, () => eventsDto.DateRange.LowerBound != null);
            Add((EventModel e) => e.EndAt == eventsDto.DateRange.UpperBound, () => eventsDto.DateRange.UpperBound != null);
        }
    }
}
