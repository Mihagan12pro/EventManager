namespace EventManager.Application.DataAccess.Queries.Bodies.UsersBookings
{
    public record CompareUserBookingQueryBody(
        Guid BookingId, 
        Guid UserId) : IQueryBody;
}
