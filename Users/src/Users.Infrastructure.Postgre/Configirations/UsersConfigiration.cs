using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain;

namespace Users.Infrastructure.Postgre.Configirations
{
    internal class UsersConfigiration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(u => u.Role)
                   .HasConversion<string>();

            builder.HasIndex(u => u.Login);
        }
    }
}
