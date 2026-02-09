using SentinelCore.DTOs.ModelDTOs;
using SentinelCore.DTOs.Pagination;
using SentinelWebApi.DTOs.Filters;

namespace SentinelWebApi.Services
{
    public interface ILogErrorService
    {
        Task<PaginatedResult<LogErrorDTO>> GetErrorListAsync(LogErrorListFilter filter, CancellationToken cancellationToken = default);

        Task<LogErrorDTO?> GetErrorByIdAsync(long errorId, CancellationToken cancellationToken = default);

        Task<bool> DeleteErrorAsync(long errorId, CancellationToken cancellationToken = default);
    }
}