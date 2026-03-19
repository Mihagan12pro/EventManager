using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Services.Events;
using EventManager.DomainModels.Events;

namespace EventsManager.Tests.Events
{
    public class EventsSeeder : ISeeder<IEnumerable<Event>>
    {
        public IEventsService EventsService { get; }

        public async Task AddSeedData()
        {
            DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);

            await EventsService.AddNew(
                new NewEventDto(
                    "Юбилей",
                    dateTime.AddDays(1),
                    dateTime.AddDays(2))
                );

            await EventsService.AddNew(
                new NewEventDto(
                    "Юбилей",
                    dateTime.AddDays(1),
                    dateTime.AddDays(2))
                );

            await EventsService.AddNew(
                new NewEventDto(
                    "Юбилей",
                    dateTime.AddDays(2),
                    dateTime.AddDays(3))
                );

            await EventsService.AddNew(
                new NewEventDto(
                    "Корпоратив",
                    dateTime.AddDays(2),
                    dateTime.AddDays(3))
                );
        }

        public async Task DeleteSeedData()
        {
            var result = await EventsService.GetEvents(null, new PaginationDto(1, 4), null);

            foreach (var item in result.Events)
            {
                await EventsService.Delete(item.Id);
            }
        }

        public async Task<bool> IsSeedEmpty()
        {
            var result = (await EventsService.GetEvents(
                null,
                new PaginationDto(),
                new DateRange(null, false, null, false)
            )).Events;

            return result.Count == 0;
        }

        public async Task<IEnumerable<Event>> GetSeededData()
        {
            var result = (await EventsService.GetEvents(null, new PaginationDto(1, 4), new DateRange(null, false, null, false)))
                .Events.Select(e => e);

            return result;
        }

        public EventsSeeder()
        {
            EventsService = new EventsService();
        }
    }
}
