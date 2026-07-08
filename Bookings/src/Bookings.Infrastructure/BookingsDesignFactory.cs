using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookings.Infrastructure
{
    internal class BookingsDesignFactory
        : IDesignTimeDbContextFactory<BookingsDbContext>
    {
        public BookingsDbContext CreateDbContext(string[] args)
        {
            string path = new DirectoryInfo(@"..\Bookings.API").FullName;

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            DbContextOptionsBuilder<BookingsDbContext> optionsBuilder = new DbContextOptionsBuilder<BookingsDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new BookingsDbContext(optionsBuilder.Options);
        }
    }
}
