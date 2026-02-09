using Microsoft.EntityFrameworkCore;
using SentinelCore.DTOs.Pagination;

namespace SentinelCore.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(this IQueryable<T> query, int page,
            int pageSize, CancellationToken cancellationToken = default)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            var totalCount = await query.CountAsync(cancellationToken);
            var results = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            return new PaginatedResult<T>
            {
                Items = results,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}