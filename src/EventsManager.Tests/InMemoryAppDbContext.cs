using EventManager.DataAccess.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Services.Tests
{
    internal class InMemoryAppDbContext : AppDbContextBase
    {
        public InMemoryAppDbContext(DbContextOptions<InMemoryAppDbContext> options) 
        {
        }
    }
}
