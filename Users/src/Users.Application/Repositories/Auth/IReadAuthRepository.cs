namespace Users.Application.Repositories.Auth
{
    public interface IReadAuthRepository
    {
        Task FindUserAsync();
    }
}
