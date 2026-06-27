using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.DTOs.Users;
using EventsManager.Tests.End2End.Extensions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace EventsManager.Tests.End2End.Bookings
{
    public class BookingsTests : E2ETests   
    {
        public BookingsTests(EventManagerAppFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Test_BookingLimits()
        {
            await SeedDefautDataAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var adminLoginReponse = await httpClient.PostAsJsonAsync(@"api\auth\login", new LoginDto("admin", "admin"), cts.Token);
            string adminToken = await adminLoginReponse.Content.ReadAsStringAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            int totalSeats = 1000;

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), totalSeats);

            var postEventResponse = await httpClient.PostAsJsonAsync(@"api\events\", newEvent, cts.Token);
            Guid eventId = await postEventResponse.Content.ExtractGuid();

            for(int i = 0; i < 10; i++)
            {
                var booking = await httpClient.PostAsJsonAsync(@$"api\events\{eventId}\book", eventId, cts.Token);

                Assert.Equal(HttpStatusCode.Accepted, booking.StatusCode);
            }

            await Task.Delay(2000);

            var finalBooking = await httpClient.PostAsJsonAsync(@$"api\events\{eventId}\book", eventId, cts.Token);

            Assert.Equal(HttpStatusCode.Conflict, finalBooking.StatusCode);
        }

        [Fact]
        public async Task Test_CancelBooking()
        {
            await SeedDefautDataAsync();

            HttpClient admin = factory.CreateClient();
            HttpClient user = factory.CreateClient();

            CancellationTokenSource cts = new CancellationTokenSource();

            var adminLoginReponse = await admin.PostAsJsonAsync(@"api\auth\login", new LoginDto("admin", "admin"), cts.Token);
            string adminToken = await adminLoginReponse.Content.ReadAsStringAsync();
            admin.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var userLoginResponse = await user.PostAsJsonAsync(@"api\auth\login", new LoginDto("user", "user"), cts.Token);
            string userToken = await userLoginResponse.Content.ReadAsStringAsync();
            user.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
            
            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);

            var postEventResponse = await admin.PostAsJsonAsync(@"api\events\", newEvent, cts.Token);
            Guid eventId = await postEventResponse.Content.ExtractGuid();

            var userResponseBooking1 = await user.PostAsJsonAsync(@$"api\events\{eventId}\book", eventId, cts.Token);
            var userContentBooking1 = await userResponseBooking1.Content.ReadAsStringAsync();
            var userAcceptedBooikng1 = JsonSerializer.Deserialize<BookingAcceptedDto>(userContentBooking1, serializerOptions);

            var userResponseBooking2 = await user.PostAsJsonAsync(@$"api\events\{eventId}\book", eventId, cts.Token);
            var userContentBooking2 = await userResponseBooking2.Content.ReadAsStringAsync();
            var userAcceptedBooikng2 = JsonSerializer.Deserialize<BookingAcceptedDto>(userContentBooking2, serializerOptions);

            var adminResponceBooking = await admin.PostAsJsonAsync(@$"api\events\{eventId}\book", eventId, cts.Token);
            var adminContentBooking = await adminResponceBooking.Content.ReadAsStringAsync();
            var adminAcceptedBooking = JsonSerializer.Deserialize<BookingAcceptedDto>(adminContentBooking, serializerOptions);

            var responseUserCanceltsBooking = await user.DeleteAsync(@$"api\bookings\{userAcceptedBooikng1.Id}", cts.Token);
            var responseAdminCancelUserBooking = await admin.DeleteAsync(@$"api\bookings\{userAcceptedBooikng2.Id}", cts.Token);
            var responseUserCancelAdminBooking = await user.DeleteAsync(@$"api\bookings\{adminAcceptedBooking.Id}", cts.Token);

            Assert.Equal(HttpStatusCode.NoContent, responseUserCanceltsBooking.StatusCode);
            Assert.Equal(HttpStatusCode.NoContent, responseAdminCancelUserBooking.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, responseUserCancelAdminBooking.StatusCode);
        }

        [Fact]
        public async Task Test_PendingToConfirmed()
        {
            await SeedDefautDataAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var loginReponse = await httpClient.PostAsJsonAsync(@"api\auth\login", new LoginDto("admin", "admin"), cts.Token);

            string token = await loginReponse.Content.ReadAsStringAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);

            var postResponse = await httpClient.PostAsJsonAsync(@"api\events\", newEvent, cts.Token);

            Assert.Equal(postResponse.StatusCode, System.Net.HttpStatusCode.Created);
            string postResponseBody = await postResponse.Content.ReadAsStringAsync();

            Guid.TryParse(postResponseBody.Trim('"'), out Guid eventId);

            var bookResponse = await httpClient.PostAsJsonAsync(@$"api\events\{eventId}\book", eventId, cts.Token);

            Assert.Equal(HttpStatusCode.Accepted, bookResponse.StatusCode);

            var acceptedContent = await bookResponse.Content.ReadAsStringAsync();

            JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            var acceptedBooking = JsonSerializer.Deserialize<BookingAcceptedDto>(acceptedContent, serializerOptions);

            var id = acceptedBooking.Id;

            await Task.Delay(5000);

            var getBooking = await httpClient.GetAsync(@$"api\bookings\{id}");
            var getBookingContent = await getBooking.Content.ReadAsStringAsync();

            var getBookingDto = JsonSerializer.Deserialize<GetBookingDto>(getBookingContent, serializerOptions);

            Assert.Equal(BookingStatus.Confirmed, getBookingDto.Status);
        }


        [Fact]
        public async Task Test_ConfirmedToRejected()
        {
            await SeedDefautDataAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var loginReponse = await httpClient.PostAsJsonAsync(@"api\auth\login", new LoginDto("admin", "admin"), cts.Token);

            string token = await loginReponse.Content.ReadAsStringAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);

            var postResponse = await httpClient.PostAsJsonAsync(@"api\events\", newEvent, cts.Token);

            Assert.Equal(postResponse.StatusCode, System.Net.HttpStatusCode.Created);
            string postResponseBody = await postResponse.Content.ReadAsStringAsync();

            Guid.TryParse(postResponseBody.Trim('"'), out Guid eventId);

            var bookResponse = await httpClient.PostAsJsonAsync(@$"api\events\{eventId}\book", eventId, cts.Token);

            Assert.Equal(HttpStatusCode.Accepted, bookResponse.StatusCode);

            var acceptedContent = await bookResponse.Content.ReadAsStringAsync();

            JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            var acceptedBooking = JsonSerializer.Deserialize<BookingAcceptedDto>(acceptedContent, serializerOptions);

            var id = acceptedBooking.Id;

            await Task.Delay(1000);

            var getConfirmedBooking = await httpClient.GetAsync(@$"api\bookings\{id}");
            var getConfirmedBookingContent = await getConfirmedBooking.Content.ReadAsStringAsync();

            var getConfirmedDto = JsonSerializer.Deserialize<GetBookingDto>(getConfirmedBookingContent, serializerOptions);

            Assert.Equal(BookingStatus.Confirmed, getConfirmedDto.Status);

            var deleteResult = await httpClient.DeleteAsync(@$"api\events\{eventId}");
            Assert.Equal(HttpStatusCode.OK, deleteResult.StatusCode);

            await Task.Delay(1000);

            var getRejectedBooking = await httpClient.GetAsync(@$"api\bookings\{id}");
            var getRejectedBookingContent = await getRejectedBooking.Content.ReadAsStringAsync();

            var getRejectedDto = JsonSerializer.Deserialize<GetBookingDto>(getRejectedBookingContent, serializerOptions);

            Assert.Equal(BookingStatus.Rejected, getRejectedDto.Status);
        }


        [Fact]
        public async Task Test_Overbooking()
        {
            await SeedDefautDataAsync();


            CancellationTokenSource cts = new CancellationTokenSource();

            var loginReponse = await httpClient.PostAsJsonAsync(@"api\auth\login", new LoginDto("admin", "admin"), cts.Token);

            string token = await loginReponse.Content.ReadAsStringAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            NewEventDto newEvent = new NewEventDto(
                "Birthday",
                DateTime.UtcNow.AddYears(1),
                DateTime.UtcNow.AddYears(1).AddDays(1),
                10
            );

            var postResponse = await httpClient.PostAsJsonAsync(@"api\events\", newEvent, cts.Token);

            Assert.Equal(postResponse.StatusCode, System.Net.HttpStatusCode.Created);
            string postResponseBody = await postResponse.Content.ReadAsStringAsync();

            Guid.TryParse(postResponseBody.Trim('"'), out Guid eventId);

            for (int i = 0; i < newEvent.TotalSeats; i++)
                await httpClient.PostAsJsonAsync(@$"api\events\{eventId}\book", eventId, cts.Token);

            var overBooked = await httpClient.PostAsJsonAsync(@$"api\events\{eventId}\book", eventId, cts.Token);

            Assert.Equal(HttpStatusCode.Conflict, overBooked.StatusCode);
        }
    }
}
