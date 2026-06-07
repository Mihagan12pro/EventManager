using EventManager.Infrastructure.PostgreSQL.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Tests.Abstractions
{
    public class DockerAppDbContext : AppDbContextBase
    {
        public DockerAppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
