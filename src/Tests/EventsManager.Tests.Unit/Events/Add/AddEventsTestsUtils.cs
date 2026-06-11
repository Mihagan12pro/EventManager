using EventManager.DTOs.Events;

namespace EventManager.Tests.Unit.Events.Add
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
            ];
        }


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
