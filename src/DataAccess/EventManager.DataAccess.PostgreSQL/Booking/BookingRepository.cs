using EventManager.Services.Bookings;

namespace EventManager.DataAccess.PostgreSQL.Booking
{
    internal class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _dbContext;

        public BookingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
