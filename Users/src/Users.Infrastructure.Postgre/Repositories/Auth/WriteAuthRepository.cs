using Microsoft.EntityFrameworkCore;
using Shared.Failures.Exceptions.WebApi.ClientErrors;
using Users.Application.Dtos.Auth;
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
            User user = null;

            try
            {
                user = new User()
                {
                    HashedPassword = register.Password,

                    UserName = new UserName(register.Login),

                    Role = register.Role
                };

                await _dbContext.Users.AddAsync(user, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch(DbUpdateException)
            {
                throw new UniqueConstraitException($"The user with login = {user.Login} is already exists!");
            }
        }

        public WriteAuthRepository(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
