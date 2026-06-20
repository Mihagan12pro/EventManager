using EventManager.Domain.Bookings;
using EventManager.Domain.Users.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Users
{
    public class UserEntity
    {
        public Guid Id { get; private set; }

        public required string HashedPassword { get; set; }

        [MinLength(3)]
        public required string Login { get; set; }

        public required Roles Role { get; set; }

        [NotMapped]
        public IEnumerable<BookingEntity> Bookings { get; set; } = null!;
    }
}
