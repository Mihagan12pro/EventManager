using Users.Application.Contracts.Auth;
using Users.Application.Repositories.Auth;
using Users.Domain;
using Users.Domain.ValueObjects;

namespace Users.Infrastructure.Postgre.Repositories.Auth
{
    internal class WriteAuthRepository : IWriteAuthRepository
    {
        private readonly UsersDbContext _dbContext;

        public async Task RegisterAsync(
            RegisterDto register, 
            CancellationToken cancellationToken)
        {
            User user = new User()
            {
                HashedPassword = register.Password,

                UserName = new UserName(register.Login),

                Role = register.Role
            };

            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public WriteAuthRepository(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
