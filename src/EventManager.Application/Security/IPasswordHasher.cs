namespace EventManager.Application.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
