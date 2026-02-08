using SentinelCore.DAL.Data.Models;

namespace SentinelCore.DAL.Repositories
{
    public interface ILogErrorRepository
    {
        Task<List<LogError>> GetAllErrorsAsync(CancellationToken cancellationToken = default);

        Task<List<LogError>> GetErrorsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        Task<List<LogError>> GetErrorsByLevel(Data.Models.LogLevel level, CancellationToken cancellationToken = default);

        Task<bool> AddNewErrorAsync(LogError logError, CancellationToken cancellationToken = default);

        Task<bool> UpdateErrorAsync(LogError logError, CancellationToken cancellationToken = default);

        Task<bool> DeleteErrorAsync(long errorId, CancellationToken cancellationToken = default);

        Task<LogError?> GetErrorByIdAsync(long errorId, CancellationToken cancellationToken = default);
    }
}