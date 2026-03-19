using EventManager.DTOs.Events;

namespace EventsManager.Tests.Events.Add
{
    public partial class AddEventsTests
    {
        public static IEnumerable<object[]> AddEventsWithException()
        {
            DateTime datetime = DateTime.Now.AddDays(1);

            return
            [
                [
                    new NewEventDto(
                        "Выпускной 11 класса",
                        null,
                        datetime
                    ),
                    "Start date time and End date time are required fields!"
                ],
                [
                    new NewEventDto(
                        "Корпоратив",
                        datetime.AddHours(6),
                        null),
                    "Start date time and End date time are required fields!"
                ],
                [
                    new NewEventDto(
                        "Корпоратив",
                        null,
                        null),
                    "Start date time and End date time are required fields!"
                ],
                [
                    new NewEventDto(
                        "Концерт",
                        datetime.AddDays(-1),
                        datetime.AddDays(2)),
                    "Too late!"
                ],
                 [
                    new NewEventDto(
                        "Концерт",
                        datetime,
                        datetime.AddDays(-1)),
                    "Too late!"
                ]
            ];
        }


        public static IEnumerable<object[]> AddEvents()
        {
            DateTime datetime = DateTime.Now.AddDays(1);

            return
            [
                [
                    new NewEventDto(
                        "Юбилей деда",

                        datetime,

                        datetime.AddHours(10))
                ],

                [
                    new NewEventDto(
                        "Золотая свадьба",

                        datetime.AddDays(10),
                        datetime.AddDays(11))
                ],

                [
                    new NewEventDto(
                        "Выпускной 11 класса",

                        new DateTime(
                            new DateOnly(DateOnly.FromDateTime(datetime).Year + 1, 6, 21),
                            new TimeOnly(18, 0, 20)),


                        new DateTime(
                            new DateOnly(DateOnly.FromDateTime(datetime).Year + 1, 6, 22),
                            new TimeOnly(8, 0, 20)))
                ],
                 [
                    new NewEventDto(
                        "Корпоратив",

                        datetime.AddHours(6),


                        datetime.AddHours(2))
                ],
            ];
        }
    }
}
