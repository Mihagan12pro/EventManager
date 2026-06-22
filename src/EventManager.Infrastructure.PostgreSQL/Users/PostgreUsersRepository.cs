using EventManager.Application.Repositories;
using EventManager.Domain.Entities.Users;
using EventManager.Domain.ValueObjects.Users;
using EventManager.DTOs.Users;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Infrastructure.PostgreSQL.Users
{
    public class PostgreUsersRepository : IUsersRepository
    {
        private readonly AppDbContextBase _dbContext;

        public async Task RegisterAsync(
            RegisterDto register,
            CancellationToken cancellationToken)
        {
            UserEntity user = new UserEntity()
            { 
                HashedPassword = register.Password,
                
                Role = register.Role, 
                
                UserName = new UserName(register.Login)
            };
            user.Validate();

            await _dbContext.AddAsync(user, cancellationToken);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Guid> GetUserIdAsync(
            LoginDto login,
            CancellationToken cancellationToken)
                => (await GetUserAsync(login, cancellationToken)).Id;

        public async Task<UserEntity> GetUserAsync(
            LoginDto login, 
            CancellationToken cancellationToken)
        {
            UserEntity user = await _dbContext.Users.FirstAsync
               (
                   u => u.Login == login.Login &&

                   u.HashedPassword == login.Password
               );

            return user;
        }

        public PostgreUsersRepository(AppDbContextBase dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
