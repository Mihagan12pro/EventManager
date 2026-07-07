using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Infrastracture.Postgre
{
    internal class EventsDesignFactory : IDesignTimeDbContextFactory<EventsDbContext>
    {
        public EventsDbContext CreateDbContext(string[] args)
        {
            string path = new DirectoryInfo(@"..\Events.API").FullName;

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            DbContextOptionsBuilder<EventsDbContext> optionsBuilder = new DbContextOptionsBuilder<EventsDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new EventsDbContext(optionsBuilder.Options);
        }
    }
}
