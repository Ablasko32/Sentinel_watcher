using SentinelCore.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SentinelCore.DAL
{
    public class SentinelContext : IdentityDbContext<AppUser>
    {
        public SentinelContext(DbContextOptions<SentinelContext> options) : base(options)
        {
        }

        public DbSet<LogError> LogError { get; set; }
    }
}