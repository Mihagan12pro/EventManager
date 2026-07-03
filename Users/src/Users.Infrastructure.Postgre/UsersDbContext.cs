using Microsoft.EntityFrameworkCore;
using Users.Domain;

namespace Users.Infrastructure.Postgre
{
    public class UsersDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
    }
}
