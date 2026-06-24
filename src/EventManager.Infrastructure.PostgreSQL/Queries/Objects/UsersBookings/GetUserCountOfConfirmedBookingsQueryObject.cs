using EventManager.Application.DataAccess.Queries;
using EventManager.Application.DataAccess.Queries.Bodies.UsersBookings;
using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.Infrastructure.PostgreSQL.DbContexts;

namespace EventManager.Infrastructure.PostgreSQL.Queries.Objects.UsersBookings
{
    internal class GetUserCountOfConfirmedBookingsQueryObject : IQueryObject<int, GetUserBookingsQueryBody>
    {
        private readonly AppDbContextBase _dbContext;

        public async Task<int> Execute(
            GetUserBookingsQueryBody queryBody,  
            CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;

            var bookings = _dbContext.Bookings.Join(
                    _dbContext.Events,
                    b => b.EventId,
                    e => e.Id,
                    (b, e) => new
                    {
                        Status = b.Status,

                        EventEnd = e.EndAt
                    }
                ).Where(o => o.Status == BookingStatus.Confirmed && o.EventEnd >= now);

            return bookings.Count();
        }

        public GetUserCountOfConfirmedBookingsQueryObject(AppDbContextBase dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
