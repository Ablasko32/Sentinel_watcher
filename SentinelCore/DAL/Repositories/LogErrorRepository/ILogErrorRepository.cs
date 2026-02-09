using SentinelCore.DAL.Data.Models;
using SentinelCore.DTOs.Pagination;
using SentinelWebApi.DTOs.Filters;

namespace SentinelCore.DAL.Repositories
{
    public interface ILogErrorRepository
    {
        Task<PaginatedResult<LogError>> GetErrorListAsync(LogErrorListFilter filter, CancellationToken cancellationToken = default);

        Task<bool> AddNewErrorAsync(LogError logError, CancellationToken cancellationToken = default);

        Task<bool> UpdateErrorAsync(LogError logError, CancellationToken cancellationToken = default);

        Task<bool> DeleteErrorAsync(long errorId, CancellationToken cancellationToken = default);

        Task<LogError?> GetErrorByIdAsync(long errorId, CancellationToken cancellationToken = default);
    }
}