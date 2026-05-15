using EventManager.DataAccess.PostgreSQL;
using EventManager.DataAccess.PostgreSQL.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Services.Tests
{
    internal class InMemoryAppDbContext : AppDbContextBase
    {
        public InMemoryAppDbContext(DbContextOptions<InMemoryAppDbContext> options) 
            : base(options)
        {
        }
    }
}
