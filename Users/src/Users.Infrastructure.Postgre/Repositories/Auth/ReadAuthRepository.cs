using Microsoft.EntityFrameworkCore;
using Users.Application.Contracts.Auth;
using Users.Application.Repositories.Auth;
using Users.Domain;

namespace Users.Infrastructure.Postgre.Repositories.Auth
{
    internal class ReadAuthRepository : IReadAuthRepository
    {
        private readonly UsersDbContext _dbContext;

        public async Task<User> FindUserAsync(
            LoginDto login, 
            CancellationToken cancellationToken)
                => await _dbContext.Users.FirstAsync(u => u.Login == login.Login && u.HashedPassword == login.Password);

        public ReadAuthRepository(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
