using SentinelCore.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace SentinelCore.DAL
{
    public class SentinelContext:DbContext
    {
        public SentinelContext(DbContextOptions<SentinelContext> options) : base(options)
        {
        }
        public DbSet<LogError> LogError { get; set; }
    }
}
