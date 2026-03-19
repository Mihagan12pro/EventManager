using EventManager.DTOs.Events;

namespace EventsManager.Tests.Events.Delete
{
    public partial class DeleteEventsTests
    {
        public static IEnumerable<object[]> AddEvents()
        {
            return
            [
                [
                    new NewEventDto(
                        string.Empty,
                        DateTime.Now.AddYears(1),
                        DateTime.Now.AddYears(2),
                        ""
                    )
                ]
            ];
        }
    }
}