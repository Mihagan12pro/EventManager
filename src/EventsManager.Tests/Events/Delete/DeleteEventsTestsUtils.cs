using EventManager.DTOs.Events;

namespace EventsManager.Tests.Events.Delete
{
    public partial class DeleteEventsTests
    {
        public static IEnumerable<object[]> AddEventsForDeleting()
        {
            DateTime now = DateTime.Now;

            return 
            [
                [
                    new NewEventDto("Юбилей деда", now.AddDays(1), now.AddDays(1).AddHours(10))
                ]
            ];
        }

        public static IEnumerable<object[]> AddNotExistsEventsForDeleting()
        {
            return
            [
                [
                    Guid.Empty
                ],

                [
                    Guid.NewGuid()
                ]
            ];
        }
    }
}
