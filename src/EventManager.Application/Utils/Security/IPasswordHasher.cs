namespace EventManager.Application.Utils.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
