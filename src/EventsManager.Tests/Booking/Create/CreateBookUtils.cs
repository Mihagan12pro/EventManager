using EventManager.DTOs.Events;

namespace EventsManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
        public static IEnumerable<object[]> AddEvents()
        {
            DateTime now = DateTime.Now.AddDays(1);

            return
            [
                [
                    new NewEventDto(
                        "Вечеринка",
                        now,
                        now.AddHours(10),
                        "Только с 18 лет")
                ],

                [
                    new NewEventDto(
                        "Юбилей деда",
                        now.AddMonths(1),
                        now.AddMonths(1).AddDays(1))
                ]
            ];
        }
    }
}
