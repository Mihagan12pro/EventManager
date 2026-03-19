namespace EventsManager.Tests
{
    public interface ISeeder
    {
        /// <summary>
        /// Add data for testing
        /// </summary>
        /// <returns></returns>
        Task AddSeedData();

        /// <summary>
        /// Removes data for testing
        /// </summary>
        /// <returns></returns>
        Task DeleteSeedData();

        Task<bool> IsSeedEmpty();
    }

    public interface ISeeder<T> : ISeeder
    {
        Task<T> GetSeededData();
    }
}
