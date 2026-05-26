using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using System.Linq.Expressions;

namespace EventManager.Tests.Integration.CRUD.Events
{
    public partial class EventsCrudTests
    {
        public static IEnumerable<NewEventDto> Seed()
        {
            DateTime datetime = DateTime.UtcNow.AddDays(1);

            var list = new List<NewEventDto>()
            {
                  new NewEventDto("birthday", datetime, datetime.AddHours(12), 10),

                  new NewEventDto("Daddy's birthday", datetime, datetime.AddHours(12), 2),

                  new NewEventDto("Daddy's brothers birthday", datetime.AddDays(2), datetime.AddDays(2).AddHours(12), 2),

                  new NewEventDto("Daddy and mom's golden marriage", datetime.AddYears(1), datetime.AddYears(1).AddHours(20), 5),

                  new NewEventDto("Rock concert", datetime, datetime.AddHours(3), 100)
            };

            return list;
        }

        public static IEnumerable<object[]> Filters()
        {
            DateTime datetime = DateTime.UtcNow.AddDays(1);

            return
            [
                [
                   (Expression<Func<EventModel, bool>>)(e => e.Title == "Birthday"),

                    new PaginationDto(1, 5),

                    1
                ],

                [
                     (Expression<Func<EventModel, bool>>)(e => e.Title == "Execution"),

                     new PaginationDto(1, 5),

                     0
                ],
                [
                     (Expression<Func<EventModel, bool>>)(e => e.Title != null),

                     new PaginationDto(1, 5),

                     5
                ],
                [
                    (Expression<Func<EventModel, bool>>)(e => e.Title.Contains("birthday")),

                    new PaginationDto(1, 5),

                     3
                ],

                [
                    (Expression<Func<EventModel, bool>>)(e => e.Title.Contains("birthday") && e.Title.StartsWith("Daddy's")),

                    new PaginationDto(3, 5),

                    2
                ],

                [
                    (Expression<Func<EventModel, bool>>)(e => e.Title.Contains("daddy")),

                    new PaginationDto(3, 5),

                    3
                ],

                [
                    (Expression<Func<EventModel, bool>>)(e => e.Title.Contains("daddy") && e.StartAt >= datetime.AddYears(1)),
                    new PaginationDto(3, 5),

                      1
                ],

                [
                     (Expression<Func<EventModel, bool>>)(e => e.Title != null),

                     new PaginationDto(10, 5),

                     5
                ],

                [
                     (Expression<Func<EventModel, bool>>)(e => e.Title.Contains("brothers")),

                     new PaginationDto(1, 5),

                     1
                ],

                [
                     (Expression<Func<EventModel, bool>>)(e => e.EndAt <= datetime.AddHours(20)),

                     new PaginationDto(1, 5),

                     3
                ],

                [
                     (Expression<Func<EventModel, bool>>)(e => e.EndAt <= datetime.AddHours(20) && e.Title.Contains("Dad")),

                     new PaginationDto(1, 5),

                     1
                ],

                [
                     (Expression<Func<EventModel, bool>>)(e => e.EndAt <= datetime.AddHours(20) && e.Title.Contains("brothers")),

                     new PaginationDto(1, 5),

                     0
                ],
            ];
        }
    }
}
