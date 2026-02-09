using Microsoft.EntityFrameworkCore;
using SentinelCore.DAL.Data.Models;
using SentinelCore.DTOs.Pagination;
using SentinelCore.Extensions;
using SentinelWebApi.DTOs.Filters;

namespace SentinelCore.DAL.Repositories
{
    public class LogErrorRepository : ILogErrorRepository
    {
        private readonly SentinelContext _context;

        public LogErrorRepository(SentinelContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<LogError>> GetErrorListAsync(LogErrorListFilter filter, CancellationToken cancellationToken=default)
        {
            var query = _context.LogError.AsNoTracking().AsQueryable();
            if (filter.Level.HasValue)
            {
                query = query.Where(e => e.Level == filter.Level.Value);
            }
            if (filter.StartDate.HasValue && filter.EndDate.HasValue)
            {
                query = query.Where(e => e.TimeStamp >= filter.StartDate.Value && e.TimeStamp <= filter.EndDate.Value);
            }
            else if (filter.StartDate.HasValue)
            {
                query = query.Where(e => e.TimeStamp >= filter.StartDate.Value);
            }
            else if (filter.EndDate.HasValue)
            {
                query = query.Where(e => e.TimeStamp <= filter.EndDate.Value);
            }

            return await query.ToPaginatedResultAsync(filter.Page, filter.PageSize, cancellationToken);
        }

        public async Task<bool> AddNewErrorAsync(LogError logError, CancellationToken cancellationToken = default)
        {
            await _context.LogError.AddAsync(logError, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> DeleteErrorAsync(long errorId, CancellationToken cancellationToken = default)
        {
            var result = await _context.LogError.Where(e => e.Id == errorId).ExecuteDeleteAsync(cancellationToken);
            return result > 0;
        }

        public async Task<LogError?> GetErrorByIdAsync(long errorId, CancellationToken cancellationToken = default)
        {
            return await _context.LogError.FirstOrDefaultAsync(e => e.Id == errorId, cancellationToken);
        }

        public async Task<bool> UpdateErrorAsync(LogError logError, CancellationToken cancellationToken = default)
        {
            _context.LogError.Update(logError);
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}