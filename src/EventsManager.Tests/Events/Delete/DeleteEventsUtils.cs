using EventManager.DTOs.Events;

namespace EventsManager.Tests.Events.Delete
{
    public partial class DeleteEventsTests
    {
        public static IEnumerable<object[]> PutDataForBadRequest()
        {
            DateTime dateTime = DateTime.Now;

            return
            [
                [
                    dateTime.AddDays(2),
                    dateTime.AddDays(1)
                ],
                [
                    dateTime.AddDays(1),
                    dateTime.AddDays(1)
                ]

            ];
        }

        public static IEnumerable<object[]> PutData()
        {
            return
            [
                [
                    0,
                    new NewEventDto(
                        "Свадьба сына",
                        DateTime.Now.AddDays(1),
                        DateTime.Now.AddDays(1).AddHours(10),
                        "Роскошная свадьба Степана и Марии")
                ]
            ];
        }
    }
}
