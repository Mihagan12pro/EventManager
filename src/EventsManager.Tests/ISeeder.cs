using System;
using System.Collections.Generic;
using System.Text;

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
    }


    public interface ISeeder<T> : ISeeder
    {
        Task<T> GetSeededData();
    }
}
