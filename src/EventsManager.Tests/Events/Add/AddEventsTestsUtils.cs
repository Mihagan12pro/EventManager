using EventManager.DTOs.Events;

namespace EventManager.Tests.Events.Add
{
    public partial class AddEventsTests
    {
        public static IEnumerable<object[]> AddBadRequest()
        {
            DateTime datetime = DateTime.Now.AddDays(1);

            return
            [
                [
                    new NewEventDto(
                        "Выпускной 11 класса",
                        null,
                        datetime,
                        10
                    ),
                    "Start date time and End date time are required fields!"
                ],
                [
                    new NewEventDto(
                        "Корпоратив",
                        datetime.AddHours(6),
                        null,
                        10),
                    "Start date time and End date time are required fields!"
                ],
                [
                    new NewEventDto(
                        "Корпоратив",
                        null,
                        null, 10),
                    "Start date time and End date time are required fields!"
                ],
                [
                    new NewEventDto(
                        "Концерт",
                        datetime.AddDays(-1),
                        datetime.AddDays(2), 10),
                    "Too late!"
                ],
                [
                    new NewEventDto(
                        "Концерт",
                        datetime,
                        datetime.AddDays(-1), 10),
                    "Too late!"
                ],
                [
                    new NewEventDto(
                        "Корпоратив",

                        datetime.AddHours(6),


                        datetime.AddHours(2), 10),
                    "Start date time must be greater than end date time!"
                ],
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

                        datetime.AddHours(10), 10)
                ],

                [
                    new NewEventDto(
                        "Золотая свадьба",

                        datetime.AddDays(10),
                        datetime.AddDays(11), 10)
                ],

                [
                    new NewEventDto(
                        "Выпускной 11 класса",

                        new DateTime(
                            new DateOnly(DateOnly.FromDateTime(datetime).Year + 1, 6, 21),
                            new TimeOnly(18, 0, 20)),


                        new DateTime(
                            new DateOnly(DateOnly.FromDateTime(datetime).Year + 1, 6, 22),
                            new TimeOnly(8, 0, 20)), 10)
                ]
            ];
        }
    }
}
