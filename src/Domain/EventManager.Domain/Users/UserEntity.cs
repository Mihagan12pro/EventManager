using System.ComponentModel.DataAnnotations;

namespace EventManager.Domain.Users
{
    public class UserEntity
    {
        public Guid Id { get; private set; }

        public required string HashedPassword { get; set; }

        [MinLength(3)]
        public required string Login { get; set; }

        public required Roles Role { get; set; }
    }
}
