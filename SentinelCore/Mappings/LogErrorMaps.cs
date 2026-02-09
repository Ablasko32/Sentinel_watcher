using SentinelCore.DAL.Data.Models;
using SentinelCore.DTOs.ModelDTOs;

namespace SentinelCore.Mappings
{
    public static class LogErrorMaps
    {
        public static LogErrorDTO ToDTO(this LogError error)
        {
            var dto = new LogErrorDTO
            {
                Id = error.Id,
                Level = error.Level,
                TimeStamp = error.TimeStamp,
                Message = error.Message,
                FilePath = error.FilePath,
                RawText = error.RawText,
                StackTrace = error.StackTrace,
                ExceptionType = error.ExceptionType,
                ExceptionMessage = error.ExceptionMessage,
                ProcessedTime = error.ProcessedTime
            };
            return dto;
        }
    }
}