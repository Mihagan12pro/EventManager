using EventManager.Domain.Entities.Bookings;
using EventManager.Domain.Entities.Users.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventManager.Domain.Entities.Users
{
    public class UserEntity
    {
        public Guid Id { get; private set; }

        [MinLength(3)]
        public required string Login { get; set; }

        public required string HashedPassword { get; set; }

        public required Roles Role { get; set; }

        [JsonIgnore]
        public List<BookingEntity> Bookings { get; set; } = null!;
    }
}
