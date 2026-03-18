namespace EventsManager.Tests
{
    public interface ISeeder
    {
        Task AddSeedData();

        Task DeleteSeedData();
    }
}
