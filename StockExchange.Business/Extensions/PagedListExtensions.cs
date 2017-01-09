using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using StockExchange.Business.Models.Paging;

namespace StockExchange.Business.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="PagedList{T}" /> class
    /// </summary>
    public static class PagedListExtensions
    {
        /// <summary>
        /// Converts an <see cref="IQueryable{T}"/> object to <see cref="PagedList{T}"/> object
        /// </summary>
        /// <typeparam name="T">Type of object in the collection</typeparam>
        /// <param name="queryable">The source collection</param>
        /// <param name="skip">Number of items to skip</param>
        /// <param name="take">Number of items take</param>
        /// <returns>Paged collection</returns>
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

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> object to <see cref="PagedList{T}"/> object
        /// </summary>
        /// <typeparam name="T">Type of object in the collection</typeparam>
        /// <param name="queryable">The source collection</param>
        /// <param name="skip">Number of items to skip</param>
        /// <param name="take">Number of items take</param>
        /// <returns>Paged collection</returns>
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
}
