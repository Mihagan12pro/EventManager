using EventManager.Domain.Events;
using EventManager.Domain.ValueObjects;
using EventManager.DTOs.Events;
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
                   (Expression<Func<EventEntity, bool>>)(e => e.Title == "Birthday"),

                    new Pagination(1, 5),

                    1
                ],

                [
                     (Expression<Func<EventEntity, bool>>)(e => e.Title == "Execution"),

                     new Pagination(1, 5),

                     0
                ],
                [
                     (Expression<Func<EventEntity, bool>>)(e => e.Title != null),

                     new Pagination(1, 5),

                     5
                ],
                [
                    (Expression<Func<EventEntity, bool>>)(e => e.Title.Contains("birthday")),

                    new Pagination(1, 5),

                     3
                ],

                [
                    (Expression<Func<EventEntity, bool>>)(e => e.Title.Contains("birthday") && e.Title.StartsWith("Daddy's")),

                    new Pagination(3, 5),

                    2
                ],

                [
                    (Expression<Func<EventEntity, bool>>)(e => e.Title.Contains("daddy")),

                    new Pagination(3, 5),

                    3
                ],

                [
                    (Expression<Func<EventEntity, bool>>)(e => e.Title.Contains("daddy") && e.StartAt >= datetime.AddYears(1)),
                    new Pagination(3, 5),

                      1
                ],

                [
                     (Expression<Func<EventEntity, bool>>)(e => e.Title != null),

                     new Pagination(10, 5),

                     5
                ],

                [
                     (Expression<Func<EventEntity, bool>>)(e => e.Title.Contains("brothers")),

                     new Pagination(1, 5),

                     1
                ],

                [
                     (Expression<Func<EventEntity, bool>>)(e => e.EndAt <= datetime.AddHours(20)),

                     new Pagination(1, 5),

                     3
                ],

                [
                     (Expression<Func<EventEntity, bool>>)(e => e.EndAt <= datetime.AddHours(20) && e.Title.Contains("Dad")),

                     new Pagination(1, 5),

                     1
                ],

                [
                     (Expression<Func<EventEntity, bool>>)(e => e.EndAt <= datetime.AddHours(20) && e.Title.Contains("brothers")),

                     new Pagination(1, 5),

                     0
                ],
            ];
        }
    }
}
