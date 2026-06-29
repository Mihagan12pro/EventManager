namespace EventManager.Application.Security
{
    public interface IJwtClaimsExtractor
    {
        string Extract(string name);
    }
}
