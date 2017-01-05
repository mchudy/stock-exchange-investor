using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.Business.Extensions
{
    public static class PagedListExtension
    {
        public static async Task<PagedList<T>> ToPagedList<T>(this IQueryable<T> queryable, int skip, int take)
        {
            var pagedList = new PagedList<T>
            {
                Skip = skip,
                Take = take,
                TotalCount = await queryable.CountAsync(),
                List = await queryable.Skip(skip).Take(take).ToListAsync()
            };
            return pagedList;
        }

        [Obsolete("Try to use IQueryable instead, this defeats the purpose of paging")]
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> queryable, int skip, int take)
        {
            var enumerable = queryable as IList<T> ?? queryable.ToList();
            var count = enumerable.Count;
            var list = enumerable.Skip(skip).Take(take).ToList();
            var pagedList = new PagedList<T>
            {
                Skip = skip,
                Take = take,
                TotalCount = count,
                List = list
            };
            return pagedList;
        }
    }

    public sealed class PagedList<T> : IReadOnlyList<T>
    {
        public List<T> List { get; set; }

        public int TotalCount { get; set; }

        public int Take { get; set; }

        public int Skip { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => List.Count;

        public T this[int index] => List[index];
    }
}
