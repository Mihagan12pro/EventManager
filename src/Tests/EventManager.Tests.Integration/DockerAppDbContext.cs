using EventManager.DataAccess.PostgreSQL.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Tests.Integration
{
    internal class DockerAppDbContext : AppDbContextBase
    {
        public DockerAppDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
