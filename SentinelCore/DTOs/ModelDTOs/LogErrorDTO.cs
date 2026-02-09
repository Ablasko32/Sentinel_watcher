using SentinelCore.DAL.Data.Models;

namespace SentinelCore.DTOs.ModelDTOs
{
    public class LogErrorDTO
    {
        public long Id { get; set; }
        public LogLevel Level { get; set; }
        public DateTime TimeStamp { get; set; }

        public required string Message { get; set; }

        public required string FilePath { get; set; }
        public required string RawText { get; set; }

        public string? StackTrace { get; set; }

        public string? ExceptionType { get; set; }

        public string? ExceptionMessage { get; set; }

        public DateTime ProcessedTime { get; set; }
    }
}
