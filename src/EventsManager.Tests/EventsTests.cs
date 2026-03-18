using EventManager.Services.Events;
using EventManager.DTOs.Events;
using EventManager.Exceptions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace EventsManager.Tests;

public class EventsTests
{
    private readonly IEventsService _eventsService;

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
