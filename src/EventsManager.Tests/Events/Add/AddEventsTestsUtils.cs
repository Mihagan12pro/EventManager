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

                    2//Counf of errors
                ],
                [
                    new NewEventDto(
                        "Корпоратив",
                        null,
                        null,
                        10),

                    2
                ],
                [
                    new NewEventDto(
                        "",
                        null,
                        null,
                        10),
                    3
                ],
                [
                    new NewEventDto(
                        string.Empty,
                        datetime.AddDays(-1),
                        datetime.AddDays(2),
                        10),

                    2
                ],
                [
                    new NewEventDto(
                        "Концерт",
                        datetime,
                        datetime.AddDays(-1),
                        10),

                    2
                ],
                [
                    new NewEventDto(
                        "Корпоратив",

                        datetime.AddHours(6),


                        datetime.AddHours(2),
                        10),

                    1
                ],
                [
                    new NewEventDto(
                        "Корпоратив",

                        datetime.AddHours(6),


                        datetime.AddHours(8),
                        0),
                    1
                ],
                 [
                    new NewEventDto(
                        null,

                        datetime.AddHours(6),


                        datetime.AddHours(2),

                        10),

                    2
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
