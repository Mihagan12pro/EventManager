using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Services.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventsManager.Tests.Events
{
    public class EventsSeeder : ISeeder
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

        public EventsSeeder()
        {
            EventsService = new EventsService();
        }
    }
}
