using SentinelCore.DAL.Repositories;
using SentinelCore.DTOs.ModelDTOs;
using SentinelCore.DTOs.Pagination;
using SentinelCore.Mappings;
using SentinelWebApi.DTOs.Filters;

namespace SentinelWebApi.Services
{
    public class LogErrorService : ILogErrorService
    {
        private readonly ILogErrorRepository _logErrorRepository;

        public LogErrorService(ILogErrorRepository logErrorRepository)
        {
            _logErrorRepository = logErrorRepository;
        }

        public async Task<PaginatedResult<LogErrorDTO>> GetErrorListAsync(LogErrorListFilter filter, CancellationToken cancellationToken=default)
        {
            var result = await _logErrorRepository.GetErrorListAsync(filter, cancellationToken);
            return new PaginatedResult<LogErrorDTO>
            {
                Items = result.Items.Select(e => e.ToDTO()).ToList(),
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages
            };
        }

        public async Task<LogErrorDTO?> GetErrorByIdAsync(long errorId, CancellationToken cancellationToken=default)
        {
            var error = await _logErrorRepository.GetErrorByIdAsync(errorId, cancellationToken);
            return error?.ToDTO();
        }

        public async Task<bool> DeleteErrorAsync(long errorId, CancellationToken cancellationToken = default)
        {
            return await _logErrorRepository.DeleteErrorAsync(errorId, cancellationToken);
        }
    }
}