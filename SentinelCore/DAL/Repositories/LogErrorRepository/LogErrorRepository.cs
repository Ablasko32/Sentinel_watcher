using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SentinelCore.DAL.Data.Models;

namespace SentinelCore.DAL.Repositories
{
    public class LogErrorRepository : ILogErrorRepository
    {
        private readonly SentinelContext _context;
        private readonly ILogger<LogErrorRepository> _logger;

        public LogErrorRepository(SentinelContext context, ILogger<LogErrorRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddNewErrorAsync(LogError logError, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.LogError.AddAsync(logError, cancellationToken);
                var result = await _context.SaveChangesAsync(cancellationToken);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add new LogError to the database.");
                throw;
            }
        }

        public async Task<bool> DeleteErrorAsync(long errorId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _context.LogError.Where(e => e.Id == errorId).ExecuteDeleteAsync(cancellationToken);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete LogError with Id {errorId} from the database.");
                throw;
            }
        }

        public async Task<LogError?> GetErrorByIdAsync(long errorId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.LogError.FirstOrDefaultAsync(e => e.Id == errorId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve LogError with Id {errorId} from the database.");
                throw;
            }
        }

        public async Task<List<LogError>> GetAllErrorsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.LogError.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all LogErrors from the database.");
                throw;
            }
        }

        public async Task<bool> UpdateErrorAsync(LogError logError, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.LogError.Update(logError);
                var result = await _context.SaveChangesAsync(cancellationToken);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update LogError with Id {logError.Id} in the database.");
                throw;
            }
        }

        public async Task<List<LogError>> GetErrorsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.LogError
                    .Where(e => e.TimeStamp >= startDate && e.TimeStamp <= endDate)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve LogErrors between {startDate} and {endDate} from the database.");
                throw;
            }
        }

        public async Task<List<LogError>> GetErrorsByLevel(Data.Models.LogLevel level, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.LogError
                    .Where(e => e.Level == level)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve LogErrors with level filter.");
                throw;
            }
        }
    }
}