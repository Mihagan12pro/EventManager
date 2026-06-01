using EventManager.DTOs.Events;

namespace EventManager.Tests.Unit.Events.Put
{
    public partial class PutEventsTests
    {
        public static IEnumerable<object[]> PutDataForBadRequest()
        {
            DateTime now = DateTime.Now;

            DateTime dateTime = new DateTime(
                new DateOnly(
                        now.Year,
                        now.Month, 
                        now.Day
                    ),

                new TimeOnly(
                        0,
                        0,
                        0
                    )
                );

            return
            [
                [
                    dateTime.AddDays(2),
                    dateTime.AddDays(1)
                ],
                [
                    dateTime.AddDays(1),
                    dateTime.AddDays(1)
                ],
                [
                    dateTime,
                    dateTime.AddHours(6)
                ],
                [
                    dateTime.AddHours(23),
                    dateTime.AddDays(1)
                ]
            ];
        }

        public static IEnumerable<object[]> PutData()
        {
            return
            [
                [
                    new NewEventDto(
                        "Свадьба сына",
                        DateTime.Now.AddDays(10),
                        DateTime.Now.AddDays(10).AddHours(10),
                        10,
                        "Роскошная свадьба Степана и Марии")
                ]
            ];
        }
    }
}
