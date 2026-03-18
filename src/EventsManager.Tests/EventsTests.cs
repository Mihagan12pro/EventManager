using EventManager.DomainModels.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Exceptions;
using EventManager.Services.Events;

namespace EventsManager.Tests
{
    public partial class EventsTests
    {
        private readonly IEventsService _eventsService;

        [Fact]
        [Trait("Get", "Get_by_id")]
        public async Task Test_Get_By_Id()
        {
            DateTime datetime = DateTime.Now.AddDays(1);

            var newEvent = new NewEventDto(
                        "Юбилей деда",

                        datetime,

                        datetime.AddHours(10));

            var result1 = await _eventsService.AddNew(newEvent);

            Guid id = result1.Value;
            Guid hiddenId = Guid.Empty;

            var result2 = (await _eventsService.GetEventById(id)).Value;
            var result3 = (await _eventsService.GetEventById(hiddenId)).Error;

            Assert.Equal(typeof(GetEventDto), result2.GetType());
            Assert.Equal(typeof(string), result3.GetType());
        }

        [Theory]
        [MemberData(nameof(GetAll))]
        [Trait("Get", "Get_all")]
        public async Task Test_Get_All(
            string? title,
            PaginationDto paginationDto, 
            DateTime? start,
            DateTime? end,
            int expectedTotalCount,
            int expectedCountOnPage)
        {
            if (!IsSeedAdded)
            {
                await AddSeedData(_eventsService);

                IsSeedAdded = true;
            }

            var result = await _eventsService.GetEvents(title, paginationDto, new DateRange(start, false, end, false));

            Assert.Equal(expectedCountOnPage, result.Events.Count);
            Assert.Equal(expectedTotalCount, result.TotalCount);
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("Add", "Success")]
        public async Task Test_Adding_New_Events_With_Correct_DateTime(NewEventDto newEventDto)
        {
            if (newEventDto.StartAt.Value.CompareTo(newEventDto.EndAt.Value) < 0)
            {
                var result = await _eventsService.AddNew(newEventDto);
                Assert.True(result.IsSuccess);
            }
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("Add", "Failure")]
        public async Task Test_Adding_New_Events_With_InCorrect_DateTime(NewEventDto newEventDto)
        {
            var value = newEventDto.StartAt.Value.CompareTo(newEventDto.EndAt.Value);

            if (newEventDto.StartAt.Value.CompareTo(newEventDto.EndAt.Value) >= 0)
            {
                var result = await _eventsService.AddNew(newEventDto);
                Assert.False(result.IsSuccess);
            }
        }

        [Theory]
        [MemberData(nameof(AddEventsWithException))]
        [Trait("Add", "Exception")]
        public async Task Test_Add_Events_With_Exception(NewEventDto dto, string expected)
        {
            var result = await Assert.ThrowsAsync<BadRequestException>(() => _eventsService.AddNew(dto));
            Assert.Equal(expected, result.Error.Message);
        }


        public EventsTests()
        {
            _eventsService = new EventsService();
        }
    }


    public partial class EventsTests
    {
        public static bool IsSeedAdded = false;

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

    public partial class EventsTests
    {
        public static async Task AddSeedData(IEventsService eventsService)
        {
            DateTime dateTime = new DateTime(new DateOnly(2027, 5, 1), new TimeOnly(20, 20)).AddYears(2);

            await eventsService.AddNew(
                new NewEventDto(
                    "Юбилей",
                    dateTime.AddDays(1),
                    dateTime.AddDays(2))
                );

            await eventsService.AddNew(
                new NewEventDto(
                    "Юбилей",
                    dateTime.AddDays(1),
                    dateTime.AddDays(2))
                );

            await eventsService.AddNew(
                new NewEventDto(
                    "Юбилей",
                    dateTime.AddDays(2),
                    dateTime.AddDays(3))
                );

            await eventsService.AddNew(
                new NewEventDto(
                    "Корпоратив",
                    dateTime.AddDays(2),
                    dateTime.AddDays(3))
                );
        }

        public static async Task DeleteSeedData(IEventsService eventsService)
        {
            var result = await eventsService.GetEvents(null, new PaginationDto(1, 4), null);

            foreach (var item in result.Events)
            {
                await eventsService.Delete(item.Id);
            }
        }
    }
}
