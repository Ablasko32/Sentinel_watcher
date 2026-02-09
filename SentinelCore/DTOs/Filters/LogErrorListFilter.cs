using SentinelCore.DAL.Data.Models;
using SentinelCore.DTOs.Filters;

namespace SentinelWebApi.DTOs.Filters
{
    public class LogErrorListFilter: Pagination
    {
        public LogLevel? Level { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
