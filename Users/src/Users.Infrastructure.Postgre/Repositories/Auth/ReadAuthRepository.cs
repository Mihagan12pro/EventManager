using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Users.Application.Dtos.Auth;
using Users.Application.Repositories.Auth;
using Users.Domain;

namespace Users.Infrastructure.Postgre.Repositories.Auth
{
    internal class ReadAuthRepository : IReadAuthRepository
    {
        private readonly UsersDbContext _dbContext;

        private readonly ILogger<ReadAuthRepository> _logger;

        public async Task<User> FindUserAsync(
            LoginDto login, 
            CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstAsync(u => u.Login == login.Login && u.HashedPassword == login.Password);

            _logger.LogInformation("The user with id = {id} has successfully logged in", user.Id);

            return user;
        }

        public ReadAuthRepository(
            UsersDbContext dbContext,
            ILogger<ReadAuthRepository> logger)
        {
            _logger = logger;

            _dbContext = dbContext;
        }
    }
}
