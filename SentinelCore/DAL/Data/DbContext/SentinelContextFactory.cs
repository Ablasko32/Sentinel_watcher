using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SentinelCore.DAL
{
    public class SentinelContextFactory : IDesignTimeDbContextFactory<SentinelContext>
    {
        public SentinelContext CreateDbContext(string[] args)
        {
            // Build configuration to read from appsettings.json in the Worker project
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SentinelFileWorker"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var optionsBuilder = new DbContextOptionsBuilder<SentinelContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new SentinelContext(optionsBuilder.Options);
        }
    }
}
