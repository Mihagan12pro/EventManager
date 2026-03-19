using EventManager.DomainModels.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Services.Events;

namespace EventsManager.Tests.Events
{
    public class EventsSeeder : ISeeder<IEnumerable<Event>>
    {
        private DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);
        private readonly IEventsService EventsService;
        private NewEventDto[] dtos;

        public async Task AddSeedData()
        {
            foreach (var d in dtos)
            {
                await EventsService.AddNew(d);
            }
        }

        public async Task DeleteSeedData()
        {
            var result0 = await EventsService.GetEvents(null, new PaginationDto(1, 4), new DateRange(null, false, null, false));

            int totalCount = result0.Events.Count;

            var result = await EventsService.GetEvents(null, new PaginationDto(1, totalCount), new DateRange(null, false, null, false));

            foreach (var item in result.Events)
            {
                await EventsService.Delete(item.Id);
            }
        }

        public async Task<IEnumerable<Event>> GetSeededData()
        {
            var result = (await EventsService.GetEvents(null, new PaginationDto(1, 4), new DateRange(null, false, null, false)))
                .Events.Select(e => e);

            return result;
        }

        public EventsSeeder(IEventsService eventsService)
        {
            EventsService = eventsService;

            dtos = new NewEventDto[]
            {
                 new NewEventDto(
                     "Юбилей",
                     dateTime.AddDays(1),
                     dateTime.AddDays(2)),

                  new NewEventDto(
                        "Юбилей",
                        dateTime.AddDays(1),
                        dateTime.AddDays(2)),

                  new NewEventDto(
                       "Юбилей",
                       dateTime.AddDays(2),
                       dateTime.AddDays(3)),
                  new NewEventDto(
                       "Корпоратив",
                       dateTime.AddDays(2),
                       dateTime.AddDays(3))
            };
        }
    }
}
