using Users.Application.Contracts.Auth;
using Users.Application.Repositories.Auth;

namespace Users.Infrastructure.Postgre.Repositories.Auth
{
    internal class WriteAuthRepository : IWriteAuthRepository
    {
        private readonly UsersDbContext _dbContext;

        public async Task RegisterAsync(
            RegisterDto register, 
            CancellationToken cancellationToken)
        {
        
        }

        public WriteAuthRepository(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
