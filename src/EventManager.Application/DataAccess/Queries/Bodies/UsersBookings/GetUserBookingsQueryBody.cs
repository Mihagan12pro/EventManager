namespace EventManager.Application.DataAccess.Queries.Bodies.UsersBookings
{
    public record GetUserBookingsQueryBody(Guid UserId) 
        : IQueryBody;
}
