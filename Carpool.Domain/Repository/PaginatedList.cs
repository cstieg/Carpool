using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Carpool.Domain.Repository
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public int TotalPageCount { get; private set; }

        public PaginatedList(int pageIndex, int pageSize, int totalCount, IEnumerable<T> source)
        {
            AddRange(source);
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public static async Task<PaginatedList<T>> CreatePaginatedListAsync(int pageIndex, int pageSize, int totalCount, IQueryable<T> source)
        {
            return new PaginatedList<T>(pageIndex, pageSize, totalCount, await source.ToListAsync());
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPageCount);
            }
        }
    }
}