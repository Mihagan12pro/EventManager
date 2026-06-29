using EventManager.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManager.Infrastructure.PostgreSQL.Users
{
    public class UsersConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(u => u.Role)
                   .HasConversion<string>();

            builder.Property(u => u.Login)
                   .HasColumnType("citext");

            builder.HasIndex(u => u.Login);
        }
    }
}
