namespace EventManager.Tests.Abstractions
{
    public interface IRealPostgreTests
    {
        Task ResetDatabaseAsync();
    }
}