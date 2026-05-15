using EventManager.DTOs.Events;

namespace EventManager.Tests.Unit.Booking.Create
{
    public partial class CreateBookTests
    {
        public static IEnumerable<object[]> AddEvents()
        {
            DateTime datetime = DateTime.Now.AddDays(10);

            return
            [
                [
                    new NewEventDto(
                        "Юбилей деда",

                        datetime,

                        datetime.AddHours(10),

                        10)
                ],

                [
                    new NewEventDto(
                        "Золотая свадьба",

                        datetime.AddDays(10),

                        datetime.AddDays(11),

                        10)
                ],

                [
                    new NewEventDto(
                        "Выпускной 11 класса",

                        new DateTime(
                            new DateOnly(DateOnly.FromDateTime(datetime).Year + 1, 6, 21),
                            new TimeOnly(18, 0, 20)),


                        new DateTime(
                            new DateOnly(DateOnly.FromDateTime(datetime).Year + 1, 6, 22),
                            new TimeOnly(8, 0, 20)),

                        10)
                ]
            ];
        }
    }
}
