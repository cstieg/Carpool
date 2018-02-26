using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Carpool.Domain.Repository
{
    public static class IQueryableExtensions
    {
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            var totalCount = await query.CountAsync();
            var collection = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return await PaginatedList<T>.CreatePaginatedListAsync(pageIndex, pageSize, totalCount, collection);
        }
    }
}
