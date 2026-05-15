using EventManager.DataAccess.PostgreSQL.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Tests.Unit
{
    internal class InMemoryAppDbContext : AppDbContextBase
    {
        public InMemoryAppDbContext(DbContextOptions<InMemoryAppDbContext> options)
           : base(options)
        {
        }
    }
}
