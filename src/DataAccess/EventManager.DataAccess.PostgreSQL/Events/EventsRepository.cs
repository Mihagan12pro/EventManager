using EventManager.Services.Events;

namespace EventManager.DataAccess.PostgreSQL.Events
{
    internal class EventsRepository : IEventsRepository
    {
        private readonly AppDbContext _dbContext;


        public EventsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
