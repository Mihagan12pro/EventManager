using EventManager.Application.DataAccess.Queries;
using EventManager.Application.DataAccess.Queries.Bodies.UsersBookings;
using EventManager.Domain.Entities.Bookings;
using EventManager.Domain.Failures.Exceptions;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.Conflict;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Infrastructure.PostgreSQL.Queries.Objects.UsersBookings
{
    internal class CompareUserBookingQuery : IQueryObject<CompareUserBookingQueryBody>
    {
        private readonly AppDbContext _dbContext;

        public async Task Execute(CompareUserBookingQueryBody queryBody, CancellationToken cancellationToken)
        {
            BookingEntity bookingEntity = await _dbContext.Bookings.FirstAsync(b => b.Id == queryBody.BookingId);

            if (bookingEntity.UserId != queryBody.UserId)
                throw new ConflictException();
        }

        public CompareUserBookingQuery(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
