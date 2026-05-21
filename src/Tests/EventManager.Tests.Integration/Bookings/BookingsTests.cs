using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace EventManager.Tests.Integration.Bookings
{
    public class BookingsTests : IntegrationTests
    {
        public BookingsTests(EventManagerAppFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Test_PendingToComfirmed()
        {
            await ResetDatabaseAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);

            var postResponse = await httpClient.PostAsJsonAsync(@"events\", newEvent, cts.Token);

            Assert.Equal(postResponse.StatusCode, System.Net.HttpStatusCode.Created);
            string postResponseBody = await postResponse.Content.ReadAsStringAsync();

            Guid.TryParse(postResponseBody.Trim('"'), out Guid eventId);

            var bookResponse = await httpClient.PostAsJsonAsync(@$"events\{eventId}\book", eventId, cts.Token);

            Assert.Equal(HttpStatusCode.Accepted, bookResponse.StatusCode);

            var acceptedContent = await bookResponse.Content.ReadAsStringAsync();

            JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            var acceptedBooking = JsonSerializer.Deserialize<BookingAcceptedDto>(acceptedContent, serializerOptions);
            
            var id = acceptedBooking.Id;
           
            await Task.Delay(5000);

            var getBooking = await httpClient.GetAsync(@$"bookings\{id}");
            var getBookingContent = await getBooking.Content.ReadAsStringAsync();

            var getBookingDto = JsonSerializer.Deserialize<GetBookingDto>(getBookingContent, serializerOptions);

            Assert.Equal(BookingStatus.Confirmed, getBookingDto.Status);
        }


        [Fact]
        public async Task Test_ConfirmedToRejected()
        {
            await ResetDatabaseAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);

            var postResponse = await httpClient.PostAsJsonAsync(@"events\", newEvent, cts.Token);

            Assert.Equal(postResponse.StatusCode, System.Net.HttpStatusCode.Created);
            string postResponseBody = await postResponse.Content.ReadAsStringAsync();

            Guid.TryParse(postResponseBody.Trim('"'), out Guid eventId);

            var bookResponse = await httpClient.PostAsJsonAsync(@$"events\{eventId}\book", eventId, cts.Token);

            Assert.Equal(HttpStatusCode.Accepted, bookResponse.StatusCode);

            var acceptedContent = await bookResponse.Content.ReadAsStringAsync();

            JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            var acceptedBooking = JsonSerializer.Deserialize<BookingAcceptedDto>(acceptedContent, serializerOptions);

            var id = acceptedBooking.Id;

            await Task.Delay(1000);

            var getConfirmedBooking = await httpClient.GetAsync(@$"bookings\{id}");
            var getConfirmedBookingContent = await getConfirmedBooking.Content.ReadAsStringAsync();

            var getConfirmedDto = JsonSerializer.Deserialize<GetBookingDto>(getConfirmedBookingContent, serializerOptions);

            Assert.Equal(BookingStatus.Confirmed, getConfirmedDto.Status);

            var deleteResult = await httpClient.DeleteAsync(@$"events\{eventId}");
            Assert.Equal(HttpStatusCode.OK, deleteResult.StatusCode);

            await Task.Delay(1000);

            var getRejectedBooking = await httpClient.GetAsync(@$"bookings\{id}");
            var getRejectedBookingContent = await getRejectedBooking.Content.ReadAsStringAsync();

            var getRejectedDto = JsonSerializer.Deserialize<GetBookingDto>(getRejectedBookingContent, serializerOptions);

            Assert.Equal(BookingStatus.Rejected, getRejectedDto.Status);
        }


        [Fact]
        public async Task Test_Overbooking()
        {
            await ResetDatabaseAsync();


            CancellationTokenSource cts = new CancellationTokenSource();

            NewEventDto newEvent = new NewEventDto(
                "Birthday",
                DateTime.UtcNow.AddYears(1),
                DateTime.UtcNow.AddYears(1).AddDays(1),
                10
            );

            var postResponse = await httpClient.PostAsJsonAsync(@"events\", newEvent, cts.Token);

            Assert.Equal(postResponse.StatusCode, System.Net.HttpStatusCode.Created);
            string postResponseBody = await postResponse.Content.ReadAsStringAsync();

            Guid.TryParse(postResponseBody.Trim('"'), out Guid eventId);

            for(int i = 0; i < newEvent.TotalSeats; i++)
                await httpClient.PostAsJsonAsync(@$"events\{eventId}\book", eventId, cts.Token);

            var overBooked = await httpClient.PostAsJsonAsync(@$"events\{eventId}\book", eventId, cts.Token);

            Assert.Equal(HttpStatusCode.Conflict, overBooked.StatusCode);
        }
    }
}
