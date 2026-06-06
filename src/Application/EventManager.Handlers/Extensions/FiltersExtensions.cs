using EventManager.Domain.Bookings;
using EventManager.Domain.Events;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventsManager.Shared.Filters;

namespace EventManager.Handlers.Extensions
{
    internal static class FiltersExtensions
    {
        public static void Add(
            this Filters<BookingModel> filters,
            GetBookingFiltersDto bookingsDto)
        {
            filters.Add((BookingModel b) => b.Status == bookingsDto.Status);
            filters.Add((BookingModel b) => b.CreatedAt == bookingsDto.CreatedAt);
            filters.Add((BookingModel b) => b.ProcessedAt == bookingsDto.ProcessedAt);
        }

        public static void Add(
            this Filters<EventModel> filters,
            GetEventsWithFiltersDto eventsDto)
        {
            filters.Add((EventModel e) => e.Title.StartsWith(eventsDto.Title), () => eventsDto.Title != null);
            filters.Add((EventModel e) => e.StartAt == eventsDto.DateRange.LowerBound, () => eventsDto.DateRange.LowerBound != null);
            filters.Add((EventModel e) => e.EndAt == eventsDto.DateRange.UpperBound, () => eventsDto.DateRange.UpperBound != null);
        }
    }
}
