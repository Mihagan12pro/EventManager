using EventManager.DTOs.Shared;

namespace EventsManager.Tests.Events.Get
{
    public partial class GetEventsTests
    {
        public static bool IsSeedAdded = false;

        public static IEnumerable<object[]> GetAllWithException()
        {
            return
            [
                [
                    -1, 1    
                ],
                [
                    -1, -1    
                ],
                [
                    1, -1    
                ]
            ];
        }

        public static IEnumerable<object[]> GetAll()
        {
            DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);

            return
            [
                [
                    null,
                    new PaginationDto(1, 2),
                    null,
                    null,
                    4,
                    2
                ],
                [
                    null,
                    new PaginationDto(10, 2),
                    null,
                    null,
                    4,
                    0
                ],
                [
                    "Юбилей",
                     new PaginationDto(1, 2),
                     null,
                     null,
                     3,
                     2
                ],
                [
                    "Юбилей",
                     new PaginationDto(10, 1),
                     null,
                     null,
                     3,
                     0
                ],
                [
                    "Юбилей",
                    new PaginationDto(2, 3),
                    dateTime.AddDays(2),
                    dateTime.AddDays(3),
                    1,
                    0
                ],
                [
                    "Юбилей",
                    new PaginationDto(1, 2),
                    dateTime.AddDays(1),
                    dateTime.AddDays(2),
                    2,
                    2
                ],
                [
                    null,
                    new PaginationDto(1, 4),
                    null,
                    null,
                    4,
                    4
                ],
                [
                    null,
                    new PaginationDto(2, 4),
                    null,
                    null,
                    4,
                    0
                ],
                [
                    null,
                    new PaginationDto(2, 1),
                    dateTime.AddDays(2),
                    null,
                    2,
                    1
                ],
                [
                    "Юбилей",
                    new PaginationDto(1, 2),
                    dateTime.AddDays(2),
                    dateTime.AddDays(1),
                    0,
                    0
                ]
            ];
        }
    }
}

